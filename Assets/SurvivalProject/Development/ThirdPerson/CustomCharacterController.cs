﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Character))]
public class CustomCharacterController : MonoBehaviour
{
    protected Character character;
    public Character Character { get { return character; } }

    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    [SerializeField]
    float m_ForwardAccelerationDamp = 0.1f;
    [SerializeField]
    float m_ForwardDecelerationDamp = 0.25f;
    [Range(1f, 4f)]
    //[SerializeField]
    //float m_GravityMultiplier = 2f;
    //[SerializeField]
    //float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    float m_GroundCheckDistance = 0.1f;

    [SerializeField]
    Transform wieldedHandgunTransform;
    [SerializeField]
    AnimationClip drawAnimationClip;

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    //float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    float m_StrafeAmount;
    float m_ForwardDampAmount;
    Vector3 m_GroundNormal;
    //float m_CapsuleHeight;
    //Vector3 m_CapsuleCenter;
    //CapsuleCollider m_Capsule;
    //bool m_Crouching;

    public static readonly string a_forwardSpeed = "ForwardSpeed";
    public static readonly string a_turnSpeed = "AngularSpeed";
    public static readonly string a_strafeSpeed = "HorizontalSpeed";
    public static readonly string a_aimingTrigger = "AimingTrigger";
    public static readonly string a_holstering = "Holstering";
    public static readonly string a_drawing = "Drawing";
    public static readonly string a_aiming = "Aiming";
    public static readonly string a_drawMultiplier = "DrawMultiplier";
    //readonly string a_grounded = "Grounded";

    public bool IsAiming
    {
        get
        {
            return m_Animator.GetBool(a_aiming);
        }
    }

    public bool IsDrawing
    {
        get
        {
            return m_Animator.GetBool(a_drawing);
        }
    }
    public bool IsHolstering
    {
        get
        {
            return m_Animator.GetBool(a_holstering);
        }
    }    

    public void MoveTurning(Vector3 move)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        //CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);

        m_ForwardAmount = move.z;

        //makes deceleration and acceleration rates differ
        if(move.z > 0)
        {
            m_ForwardDampAmount = m_ForwardAccelerationDamp;
        }
        else
        {
            m_ForwardDampAmount = m_ForwardDecelerationDamp;
        }

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        //if (!m_IsGrounded)
        //{
        //    HandleAirborneMovement();
        //}

        //PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        UpdateAnimator(move, false);
    }

    public void MoveStrafing(Vector3 input)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (input.magnitude > 1f) input.Normalize();
        input = transform.InverseTransformDirection(input);
        //CheckGroundStatus();
        input = Vector3.ProjectOnPlane(input, m_GroundNormal);
        m_StrafeAmount = input.x;

        m_ForwardAmount = input.z;

        //makes deceleration and acceleration rates differ
        if (input.z > 0)
        {
            m_ForwardDampAmount = m_ForwardAccelerationDamp;
        }
        else
        {
            m_ForwardDampAmount = m_ForwardDecelerationDamp;
        }

        // control and velocity handling is different when grounded and airborne:
        //if (!m_IsGrounded)
        //{
        //    HandleAirborneMovement();
        //}

        //PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        UpdateAnimator(input, true);
    }

    public void DrawWeapon()
    {
        float drawMultiplier = drawAnimationClip.length / character.EquippedHolster.DrawTime;

        m_Animator.SetFloat(a_drawMultiplier, drawMultiplier);

        m_Animator.SetTrigger(a_aimingTrigger);
        Character.DrawFromHolster();

        Character.EquippedHolster.EquippedFirearm.transform.SetParent(wieldedHandgunTransform, false);
    }

    public void HolsterWeapon()
    {
        float drawMultiplier = drawAnimationClip.length / character.EquippedHolster.DrawTime;

        m_Animator.SetFloat(a_drawMultiplier, drawMultiplier);

        m_Animator.SetTrigger(a_aimingTrigger);

        Character.WieldedFirearm.transform.SetParent(Character.EquippedHolster.transform, false);
        Character.HolsterFirearm();
    }
        
    void UpdateAnimator(Vector3 movement, bool strafe)
    {
        // update the animator parameters
        m_Animator.SetFloat(a_forwardSpeed, m_ForwardAmount, m_ForwardDampAmount, Time.deltaTime);

        if(strafe)
            m_Animator.SetFloat(a_strafeSpeed, m_StrafeAmount, m_ForwardDampAmount, Time.deltaTime);
        else
            m_Animator.SetFloat(a_turnSpeed, m_TurnAmount, m_ForwardDampAmount, Time.deltaTime);

        //m_Animator.SetBool("Crouch", m_Crouching);
        //m_Animator.SetBool(a_grounded, m_IsGrounded);

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (m_IsGrounded && movement.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    public void ApplyExtraTurnRotation(float angle)
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, angle * turnSpeed * Time.deltaTime, 0);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        //m_Capsule = GetComponent<CapsuleCollider>();
        //m_CapsuleHeight = m_Capsule.height;
        //m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        //m_OrigGroundCheckDistance = m_GroundCheckDistance;

        character = GetComponent<Character>();
    }

    //public void OnAnimatorMove()
    //{
    //    // we implement this function to override the default root motion.
    //    // this allows us to modify the positional speed before it's applied.
    //    if (m_IsGrounded && Time.deltaTime > 0)
    //    {
    //        Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

    //        // we preserve the existing y part of the current velocity.
    //        v.y = m_Rigidbody.velocity.y;
    //        m_Rigidbody.velocity = v;
    //    }
    //}

    //void PreventStandingInLowHeadroom()
    //{
    //    // prevent standing up in crouch-only zones
    //    if (!m_Crouching)
    //    {
    //        Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
    //        float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
    //        if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
    //        {
    //            m_Crouching = true;
    //        }
    //    }
    //}

    //void ScaleCapsuleForCrouching(bool crouch)
    //{
    //    if (m_IsGrounded && crouch)
    //    {
    //        if (m_Crouching) return;
    //        m_Capsule.height = m_Capsule.height / 2f;
    //        m_Capsule.center = m_Capsule.center / 2f;
    //        m_Crouching = true;
    //    }
    //    else
    //    {
    //        Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
    //        float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
    //        if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
    //        {
    //            m_Crouching = true;
    //            return;
    //        }
    //        m_Capsule.height = m_CapsuleHeight;
    //        m_Capsule.center = m_CapsuleCenter;
    //        m_Crouching = false;
    //    }
    //}    
}
