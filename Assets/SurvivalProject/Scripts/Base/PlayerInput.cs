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
    protected bool operationEnabled;
    protected bool holdingTrigger;
    protected bool drawing;

    [SerializeField]
    private Transform weaponCameraOperationTransform;
    [SerializeField]
    private Transform weaponCameraChamberTransform;

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
            //start drawing
            if (!drawing && !enabledCrosshair)
            {
                character.DrawFromHolster();

                drawing = true;                
            }

            //enable crosshair when drawing
            if (!enabledCrosshair)
            {                       
                crosshair.Show();
                SetMovementState(MovementState.Walking);

                enabledCrosshair = true;
            }

            //activates operation and weapon camera
            if (drawing)
            {
                if (character.WieldedFirearm != null)
                {
                    UIController.Instance.SetWeaponCamera(true);
                    operationEnabled = true;

                    drawing = false;
                }
            }
        }

        if (XboxOneInput.GetAxis(XboxOneAxis.LT) == 0)
        {
            if (enabledCrosshair)
            {
                UIController.Instance.SetWeaponCamera(false);
                operationEnabled = false;
                crosshair.Hide();
                SetMovementState(MovementState.Running);                
                character.HolsterFirearm();
                drawing = false;

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
                SceneManager.Instance.MainCamera.SetOffset(cameraOffset);
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

    protected void HandleOperation()
    {        
        if(operationEnabled)
        {
            //toggle safety
            if (XboxOneInput.GetButtonDown(XboxOneButton.B))
            {
                Character.WieldedFirearm.ToggleSafety();
            }

            //toggle hammer
            if (XboxOneInput.GetButtonDown(XboxOneButton.RB))
            {
                Character.WieldedFirearm.Cock();
            }

            //pull slide
            //will release ammo from chamber if loaded
            if (XboxOneInput.GetButtonDown(XboxOneButton.Y))
            {
                //SceneManager.Instance.SetWeaponCameraTransform(weaponCameraChamberTransform, false);
                Character.WieldedFirearm.PullSlide();                
            }

            //release slide
            //will load chamber if magazine is loaded
            if (XboxOneInput.GetButtonUp(XboxOneButton.Y))
            {
                //SceneManager.Instance.SetWeaponCameraTransform(weaponCameraOperationTransform, false);
                Character.WieldedFirearm.ReleaseSlide();                
            }

            if(enabledCrosshair)
            {
                //shoots when pressing RT
                if (XboxOneInput.GetAxis(XboxOneAxis.RT) > 0)
                {
                    if (!holdingTrigger)
                    {
                        Character.WieldedFirearm.PullTrigger();
                        holdingTrigger = true;
                    }
                }
                else
                {
                    holdingTrigger = false;
                }
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        enabledCrosshair = false;
        drawing = false;
    }

    void Update()
    {
        HandleMovement();
        HandleAim();
        HandleOperation();
    }
}
