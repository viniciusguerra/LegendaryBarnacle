using UnityEngine;
using System.Collections;

public enum MovementState
{
    Walking,
    Running
}

[RequireComponent(typeof(CharacterController))]
public class CharacterInput : MonoBehaviour
{
    public float walkSpeed;
    public float averageSpeed;
    public float currentSpeed;
    public float maxDegreesDelta = 300;

    private CharacterController characterController;

    [SerializeField]
    private Crosshair crosshair;
    private bool enabledCrosshair;

    //used for calculating movement direction
    private Vector3 lastPosition;
    //used for interpolating rotation
    private Quaternion targetRotation;
    private float stickRotationThreshold = 0.05f;
    private float movementLookThreshold = 0.05f;

    private void Aim()
    {
        if (XboxOneInput.GetAxis(XboxOneAxis.LT) > 0)
        {
            if (!enabledCrosshair)
            {
                crosshair.Show();
                SetMovementState(MovementState.Walking);
                enabledCrosshair = true;
            }
        }

        if (XboxOneInput.GetAxis(XboxOneAxis.LT) == 0)
        {
            if (enabledCrosshair)
            {
                crosshair.Hide();
                SetMovementState(MovementState.Running);
                enabledCrosshair = false;
            }
        }

        if (enabledCrosshair)
        {
            //looks towards crosshair
            Vector3 lookDirection = new Vector3(crosshair.transform.position.x, transform.position.y, crosshair.transform.position.z);
            transform.LookAt(lookDirection);
        }
        else
        {
            float horizontalRotationAxis = XboxOneInput.GetAxis(XboxOneAxis.RightThumbX);
            float verticalRotationAxis = XboxOneInput.GetAxis(XboxOneAxis.RightThumbY);
            bool hasAimingInput = horizontalRotationAxis != 0 || verticalRotationAxis != 0;

            if (hasAimingInput && (Mathf.Abs(horizontalRotationAxis) > stickRotationThreshold || Mathf.Abs(verticalRotationAxis) > stickRotationThreshold))
            {
                SetMovementState(MovementState.Walking);

                //rotates with right stick input
                targetRotation = Quaternion.LookRotation(new Vector3(horizontalRotationAxis, 0, verticalRotationAxis));

                Vector3 cameraOffset = new Vector3(horizontalRotationAxis, 0, verticalRotationAxis);
                MainCamera.Instance.SetOffset(cameraOffset);
            }
            else
            {
                SetMovementState(MovementState.Running);

                //looks towards movement if it is significant enough
                if((lastPosition - transform.position).magnitude > movementLookThreshold)
                    targetRotation = Quaternion.LookRotation(transform.position - lastPosition, Vector3.up);
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta * Time.deltaTime);            
        }
    }

    private void SetMovementState(MovementState state)
    {
        switch (state)
        {
            case MovementState.Walking:
                {
                    currentSpeed = walkSpeed;
                    break;
                }
            case MovementState.Running:
                {
                    currentSpeed = averageSpeed;
                    break;
                }
            default:
                break;
        }

    }

    private void Move()
    {
        lastPosition = transform.position;

        Vector3 movementDirection = new Vector3(XboxOneInput.GetAxis(XboxOneAxis.LeftThumbX), 0, XboxOneInput.GetAxis(XboxOneAxis.LeftThumbY)) * currentSpeed * Time.deltaTime;

        characterController.Move(movementDirection);
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        enabledCrosshair = false;
    }

    void Update()
    {
        Move();
        Aim();
    }
}
