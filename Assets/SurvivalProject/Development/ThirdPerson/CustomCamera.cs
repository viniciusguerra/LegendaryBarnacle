using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Cameras;

public enum CustomCameraMode
{
    ThirdPerson,
    FirstPerson
}

public delegate void CameraModeChangeDelegate(CustomCameraMode previousMode, CustomCameraMode currentMode);

public class CustomCamera : PivotBasedCameraRig
{
    //Based on UnityStandardAssets.Cameras.FreeLookCam

    // This script is designed to be placed on the root object of a camera rig,
    // comprising 3 gameobjects, each parented to the next:

    // 	Camera Rig
    // 		Pivot
    // 			Camera

    [SerializeField]
    private CustomCharacterController characterController;
    [SerializeField]
    private CustomCameraMode cameraMode;
    [SerializeField]
    private float cameraTranslateTime = 0.6f;

    public CustomCameraMode CameraMode
    {
        get { return cameraMode; }
        set
        {
            CustomCameraMode previousMode = cameraMode;

            cameraMode = value;

            OnCameraModeChange(previousMode, value);
        }
    }

    public event CameraModeChangeDelegate OnCameraModeChange;

    [Header("Third Person")]
    [SerializeField]
    private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
    [Range(0f, 10f)]
    [SerializeField]
    private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
    [SerializeField]
    private float m_TurnSmoothing = 0.1f;                // How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness
    [SerializeField]
    private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    [SerializeField]
    private float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
    //[SerializeField]
    //private bool m_LockCursor = false;                   // Whether the cursor should be hidden and locked.
    //[SerializeField]
    //private bool m_VerticalAutoReturn = false;           // set wether or not the vertical axis should auto return

    [Header("First Person")]
    [SerializeField]
    private Transform m_FirstPersonTargetTransform;
    [SerializeField]
    private float m_MaxHorizontalAngle;
    [SerializeField]
    private float m_MaxVerticalAngle;
    [SerializeField]
    private float m_FirstPersionDamping;

    private float m_LookAngle;                    // The rig's y axis rotation.
    private float m_TiltAngle;                    // The pivot's x axis rotation.
    private const float k_LookDistance = 100f;    // How far in front of the pivot the character's look target is.
    private Vector3 m_PivotEulers;
    private Quaternion m_PivotTargetRot;
    private Quaternion m_TransformTargetRot;

    private Vector3 thirdPersonPivotLocalPosition;
    private Quaternion thirdPersonPivotLocalRotation;
    private Vector3 thirdPersonCameraLocalPosition;
    private Quaternion thirdPersonCameraLocalRotation;

    protected void SetCameraMode(CustomCameraMode previousValue, CustomCameraMode value)
    {
        if (previousValue != value)
        {
            switch (value)
            {
                case CustomCameraMode.ThirdPerson:
                {
                    m_Pivot.localPosition = thirdPersonPivotLocalPosition;
                    m_Pivot.localRotation = thirdPersonPivotLocalRotation;
                    m_Cam.localPosition = thirdPersonCameraLocalPosition;
                    m_Cam.localRotation = thirdPersonCameraLocalRotation;

                    //transform.position = Vector3.Lerp(transform.position, m_Target.position, cameraTranslateTime * Time.deltaTime);
                    iTween.MoveTo(gameObject, m_Target.position, cameraTranslateTime);
                    break;
                }
                case CustomCameraMode.FirstPerson:
                {
                    m_Pivot.localPosition = Vector3.zero;
                    m_Pivot.localRotation = Quaternion.identity;
                    m_Cam.localPosition = Vector3.zero;
                    m_Cam.localRotation = Quaternion.identity;

                    //transform.position = Vector3.Lerp(transform.position, m_FirstPersonTargetTransform.position, cameraTranslateTime * Time.deltaTime);
                    iTween.MoveTo(gameObject, m_FirstPersonTargetTransform.position, cameraTranslateTime);
                    break;
                }
                default:
                break;
            }
        }
    }

    protected override void FollowTarget(float deltaTime)
    {
        switch (cameraMode)
        {
            case CustomCameraMode.ThirdPerson:
            {
                HandleThirdPersonControl();

                if (m_Target == null) return;
                // Move the rig towards target position.               

                transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime * m_MoveSpeed);                

                break;
            }
            case CustomCameraMode.FirstPerson:
            {
                HandleThirdPersonControl();

                //HandleFirstPersonControl();

                transform.position = m_FirstPersonTargetTransform.position;

                break;
            }
            default:
            break;
        }        
    }

    private void HandleThirdPersonControl()
    {
        if (Time.timeScale < float.Epsilon)
            return;

        // Read the user input
        var x = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbX);
        var y = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbY);

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
        m_LookAngle += x * m_TurnSpeed;

        // Rotate the rig (the root object) around Y axis only:
        m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

        //if (m_VerticalAutoReturn)
        //{
        //    // For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
        //    // on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
        //    // we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
        //    m_TiltAngle = y > 0 ? Mathf.Lerp(0, -m_TiltMin, y) : Mathf.Lerp(0, m_TiltMax, -y);
        //}
        //else
        //{

        // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
        m_TiltAngle -= y * m_TurnSpeed;
        // and make sure the new value is within the tilt range
        m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);

        //}

        // Tilt input around X is applied to the pivot (the child of this object)
        m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y, m_PivotEulers.z);

        if (m_TurnSmoothing > 0)
        {
            m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
        }
        else
        {
            m_Pivot.localRotation = m_PivotTargetRot;
            transform.localRotation = m_TransformTargetRot;
        }
    }

    private void HandleFirstPersonControl()
    {
        if (Time.timeScale < float.Epsilon)
            return;

        // Read the user input
        var horizontalInput = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbX);
        var verticalInput = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbY);

        Vector3 targetRotation = new Vector3(
            Mathf.Clamp(verticalInput, -m_MaxVerticalAngle, m_MaxVerticalAngle),
            Mathf.Clamp(horizontalInput, -m_MaxHorizontalAngle, m_MaxHorizontalAngle),            
            0);        

        m_Pivot.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetRotation), m_FirstPersionDamping * Time.deltaTime);
    }

    protected override void Awake()
    {
        base.Awake();

        // Lock or unlock the cursor.
        //Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        //Cursor.visible = !m_LockCursor;

        if (!Application.isEditor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        m_PivotEulers = m_Pivot.rotation.eulerAngles;

        m_PivotTargetRot = m_Pivot.transform.localRotation;
        m_TransformTargetRot = transform.localRotation;

        cameraMode = CustomCameraMode.ThirdPerson;

        thirdPersonPivotLocalPosition = m_Pivot.localPosition;
        thirdPersonPivotLocalRotation = m_Pivot.localRotation;
        thirdPersonCameraLocalPosition = m_Cam.localPosition;
        thirdPersonCameraLocalRotation = m_Cam.localRotation;

        OnCameraModeChange += SetCameraMode;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

