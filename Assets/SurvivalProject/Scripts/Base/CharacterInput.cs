using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomCharacterController))]
public class CharacterInput : MonoBehaviour
{
    protected CustomCharacterController characterController;  
    public CustomCharacterController CharacterController { get { return characterController; } }
    public Character Character { get { return characterController.Character; } }

    //public float walkSpeed;
    //public float averageSpeed;
    //public float currentSpeed;
    //public float maxDegreesDelta = 300;

    //used for calculating movement direction
    protected Vector3 lastPosition;
    //used for interpolating rotation
    protected Quaternion targetRotation;
    protected float stickRotationThreshold = 0.05f;
    protected float movementLookThreshold = 0.05f;

    protected void MoveTurning(Vector3 movementDirection)
    {
        characterController.MoveTurning(movementDirection);
    }

    protected void MoveStrafing(Vector3 movementDirection)
    {
        characterController.MoveStrafing(movementDirection);
    }

    protected void TriggerAiming()
    {
        characterController.DrawWeapon();
    }

    //protected void Aim(Quaternion targetRotation)
    //{
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta * Time.deltaTime);
    //}

    //protected void SetMovementState(MovementState state)
    //{
    //    switch (state)
    //    {
    //        case MovementState.Walking:
    //            {
    //                currentSpeed = walkSpeed;
    //                break;
    //            }
    //        case MovementState.Running:
    //            {
    //                currentSpeed = averageSpeed;
    //                break;
    //            }
    //        default:
    //            break;
    //    }
    //}

    protected virtual void Start()
    {
        characterController = GetComponent<CustomCharacterController>();
    }    
}
