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

    #region Properties
    [SerializeField]
    private CustomCharacterController m_PlayerCharacterController;
    [SerializeField]
    private CustomCameraMode m_CameraMode;

    public CustomCameraMode CameraMode
    {
        get { return m_CameraMode; }
        set
        {
            CustomCameraMode previousMode = m_CameraMode;

            m_CameraMode = value;

            OnCameraModeChange(previousMode, value);
        }
    }

    public event CameraModeChangeDelegate OnCameraModeChange;

    [Header("Third Person")]
    [SerializeField]
    private Vector3 m_ThirdPersonPivotPosition;
    [SerializeField]
    private Vector3 m_ThirdPersonCameraPosition;
    [SerializeField]
    private Vector3 m_ThirdPersonCameraRotation;
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
    private Vector3 m_FirstPersonPivotPosition;
    [SerializeField]
    private Vector3 m_FirstPersonCameraPosition;
    [SerializeField]
    private Vector3 m_FirstPersonCameraRotation;

    private Transform m_FrontSightTransform;
    private Transform m_RearSightTransform;
    //[SerializeField]
    //private float m_MaxHorizontalAngle;
    //[SerializeField]
    //private float m_MaxVerticalAngle;
    //[SerializeField]
    //private float m_FirstPersionDamping;

    private float m_RigHorizontalAngle;                    // The rig's y axis rotation.
    private float m_PivotVerticalAngle;                    // The pivot's x axis rotation.
    private const float k_LookDistance = 100f;    // How far in front of the pivot the character's look target is.
    private Quaternion m_PivotTargetRotation;
    private Quaternion m_RigTargetRotation;
    #endregion

    #region Methods
    protected void SetCameraMode(CustomCameraMode previousValue, CustomCameraMode value)
    {
        if (previousValue != value)
        {
            switch (value)
            {
                case CustomCameraMode.ThirdPerson:
                {
                    SetTarget(m_PlayerCharacterController.transform);

                    break;
                }
                case CustomCameraMode.FirstPerson:
                {
                    m_FrontSightTransform = m_PlayerCharacterController.Character.WieldedFirearm.transform.FindChild("FrontSightTransform");
                    m_RearSightTransform = m_PlayerCharacterController.Character.WieldedFirearm.transform.FindChild("RearSightTransform");

                    SetTarget(m_FrontSightTransform);

                    break;
                }
                default:
                break;
            }
        }
    }    

    private void AdjustPivotAndCamera()
    {
        Vector3 targetPivotPosition, targetCameraPosition, targetCameraRotation;

        switch (m_CameraMode)
        {
            case CustomCameraMode.ThirdPerson:
            {
                targetPivotPosition = m_ThirdPersonPivotPosition;
                targetCameraPosition = m_ThirdPersonCameraPosition;
                targetCameraRotation = m_ThirdPersonCameraRotation;

                break;
            }
            case CustomCameraMode.FirstPerson:
            {
                targetPivotPosition = m_FirstPersonPivotPosition;
                targetCameraPosition = m_FirstPersonCameraPosition;
                targetCameraRotation = m_FirstPersonCameraRotation;

                break;
            }
            default:
                return;
        }

        if (m_Pivot.localPosition != targetPivotPosition)
            m_Pivot.localPosition = Vector3.Lerp(m_Pivot.localPosition, targetPivotPosition, Time.deltaTime * m_MoveSpeed);

        if (m_Cam.localPosition != targetCameraPosition)
            m_Cam.localPosition = Vector3.Lerp(m_Cam.localPosition, targetCameraPosition, Time.deltaTime * m_MoveSpeed);

        if (m_Cam.localRotation.eulerAngles != targetCameraRotation)
            m_Cam.localRotation = Quaternion.Slerp(m_Cam.localRotation, Quaternion.Euler(targetCameraRotation), m_TurnSmoothing * Time.deltaTime);
    }

    private void HandleThirdPersonControl()
    {
        if (Time.timeScale < float.Epsilon)
            return;

        // Read the user input
        var horizontalInput = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbX);
        var verticalInput = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbY);

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
        m_RigHorizontalAngle += horizontalInput * m_TurnSpeed;

        m_PivotVerticalAngle -= verticalInput * m_TurnSpeed;
        // and make sure the new value is within the tilt range
        m_PivotVerticalAngle = Mathf.Clamp(m_PivotVerticalAngle, -m_TiltMin, m_TiltMax);

        // Rotate the rig (the root object) around Y axis only:
        m_RigTargetRotation = Quaternion.Euler(0f, m_RigHorizontalAngle, 0f);

        // Tilt input around X is applied to the pivot (the child of this object)
        m_PivotTargetRotation = Quaternion.Euler(m_PivotVerticalAngle, 0, 0);

        if (m_TurnSmoothing > 0)
        {
            m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRotation, m_TurnSmoothing * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_RigTargetRotation, m_TurnSmoothing * Time.deltaTime);
        }
        else
        {
            m_Pivot.localRotation = m_PivotTargetRotation;
            transform.localRotation = m_RigTargetRotation;
        }
    }

    protected override void FollowTarget(float deltaTime)
    {
        switch (m_CameraMode)
        {
            case CustomCameraMode.ThirdPerson:
            {
                HandleThirdPersonControl();

                break;
            }
            case CustomCameraMode.FirstPerson:
            {
                transform.rotation = Quaternion.LookRotation(m_FrontSightTransform.position - m_RearSightTransform.position);

                break;
            }
            default:
            break;
        }        

        if (m_Target != null)
            transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime * 50);

        AdjustPivotAndCamera();
    }
    #endregion

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();

        if (!Application.isEditor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        m_CameraMode = CustomCameraMode.ThirdPerson;

        OnCameraModeChange += SetCameraMode;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
}

