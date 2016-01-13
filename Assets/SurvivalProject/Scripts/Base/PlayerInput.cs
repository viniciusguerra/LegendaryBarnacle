using UnityEngine;
using System.Collections;

public enum MovementState
{
    Walking,
    Running
}

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : CharacterInput
{
    [SerializeField]
    protected Crosshair crosshair;
    public Crosshair Crosshair { get { return crosshair; } }

    protected bool enabledCrosshair;
    protected bool holdingTrigger;

    protected void HandleMovement()
    {
        lastPosition = transform.position;

        float horizontalMovementAxis = XboxOneInput.GetAxis(XboxOneAxis.LeftThumbX);
        float verticalMovementAxis = XboxOneInput.GetAxis(XboxOneAxis.LeftThumbY);
        bool hasMovementInput = horizontalMovementAxis != 0 || verticalMovementAxis != 0;

        Vector3 movementDirection = new Vector3(horizontalMovementAxis, 0, verticalMovementAxis) * currentSpeed * Time.deltaTime;

        if(hasMovementInput)
            Move(movementDirection);
    }

    protected void HandleAim()
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

            if (XboxOneInput.GetAxis(XboxOneAxis.RT) > 0)
            {
                if (!holdingTrigger)
                {
                    Character.EquippedFirearm.Controller.PullTrigger();
                    holdingTrigger = true;
                }
            }
            else
            {
                holdingTrigger = false;
            }
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
                if ((lastPosition - transform.position).magnitude > movementLookThreshold)
                    targetRotation = Quaternion.LookRotation(transform.position - lastPosition, Vector3.up);
            }

            Aim(targetRotation);
        }
    }

    protected override void Start()
    {
        base.Start();
        enabledCrosshair = false;
    }

    void Update()
    {
        HandleMovement();
        HandleAim();
    }
}
