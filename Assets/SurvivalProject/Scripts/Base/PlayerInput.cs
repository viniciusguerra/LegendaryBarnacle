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
    protected bool weaponCameraAtChamberTransform;
    protected bool weaponCameraAtMagazineTransform;

    [SerializeField]
    private float buttonHoldTime = 0.1f;

    [SerializeField]
    private LootArea lootArea;

    protected void HandleMovement()
    {
        lastPosition = transform.position;

        float horizontalMovementAxis = XboxOneInput.Instance.GetAxis(XboxOneAxis.LeftThumbX);
        float verticalMovementAxis = XboxOneInput.Instance.GetAxis(XboxOneAxis.LeftThumbY);
        bool hasMovementInput = horizontalMovementAxis != 0 || verticalMovementAxis != 0;

        Vector3 movementDirection = new Vector3(horizontalMovementAxis, 0, verticalMovementAxis) * currentSpeed * Time.deltaTime;

        if (hasMovementInput)
            Move(movementDirection);
    }

    protected void HandleAim()
    {
        if (XboxOneInput.Instance.GetAxis(XboxOneAxis.LT) > 0)
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
                    UIController.Instance.HUD.SetWeaponCameraVisibility(true);
                    operationEnabled = true;

                    drawing = false;
                }
            }
        }

        if (XboxOneInput.Instance.GetAxis(XboxOneAxis.LT) == 0)
        {
            if (enabledCrosshair)
            {
                UIController.Instance.HUD.SetWeaponCameraVisibility(false);
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
            float horizontalRotationAxis = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbX);
            float verticalRotationAxis = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbY);
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
        if (operationEnabled)
        {
            //toggle safety
            if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.B))
            {
                Character.WieldedFirearm.ToggleSafety();
            }

            //toggle hammer
            if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.RB))
            {
                Character.WieldedFirearm.Cock();
            }

            //pull slide
            //will release ammo from chamber if loaded
            //TODO: Fix slide release not working
            if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.Y))
                XboxOneInput.Instance.OnButtonHeld(XboxOneButton.Y, buttonHoldTime, Character.WieldedFirearm.FullSlidePull, Character.WieldedFirearm.HalfSlideToggle);

            //Set Weapon Camera to check slide
            if (weaponCameraAtChamberTransform == false && Character.WieldedFirearm.SlideHalfBack)
            {
                SceneManager.Instance.SetWeaponCameraTransform(Character.WieldedFirearm.ChamberTransform, false);
                weaponCameraAtMagazineTransform = false;
                weaponCameraAtChamberTransform = true;
            }

            //release slide
            //will load chamber if magazine is loaded
            if (XboxOneInput.Instance.GetButtonUp(XboxOneButton.Y) && Character.WieldedFirearm.SlideFullBack)
            {
                //SceneManager.Instance.SetWeaponCameraTransform(weaponCameraOperationTransform, false);
                Character.WieldedFirearm.ReleaseSlide();
            }

            //Set Weapon Camera back if slide is released
            if (weaponCameraAtChamberTransform == true && (!Character.WieldedFirearm.SlideHalfBack && !Character.WieldedFirearm.SlideFullBack))
            {
                SceneManager.Instance.SetWeaponCameraTransform(Character.WieldedFirearm.OperationTransform, false);
                weaponCameraAtChamberTransform = false;
            }

            if (enabledCrosshair)
            {
                //shoots when pressing RT
                if (XboxOneInput.Instance.GetAxis(XboxOneAxis.RT) > 0)
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

    public void HandleCharacterMenuInput()
    {

    }

    public void HandleReload()
    {
        if (operationEnabled)
        {
            if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.X))
                XboxOneInput.Instance.OnButtonHeld(XboxOneButton.X, buttonHoldTime, UIController.Instance.HUD.ReloadMenu.Toggle, Character.WieldedFirearm.ToggleCheckMagazine);

            if (weaponCameraAtMagazineTransform == false && !Character.WieldedFirearm.MagazineIsAttached)
            {
                SceneManager.Instance.SetWeaponCameraTransform(Character.WieldedFirearm.MagazineTransform, false);
                weaponCameraAtMagazineTransform = true;
                weaponCameraAtChamberTransform = false;
            }

            if (weaponCameraAtMagazineTransform == true && Character.WieldedFirearm.MagazineIsAttached)
            {
                SceneManager.Instance.SetWeaponCameraTransform(Character.WieldedFirearm.OperationTransform, false);
                weaponCameraAtMagazineTransform = false;
            }
        }
        else
        {
            if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.X))
                XboxOneInput.Instance.OnButtonHeld(XboxOneButton.X, buttonHoldTime, UIController.Instance.HUD.ReloadMenu.Toggle, null);
        }
    }

    public void HandleCharacterUnarmedInput()
    {
        if (!operationEnabled && !enabledCrosshair)
        {
            if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.Y))
                lootArea.CycleItems();

            if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.A))
                lootArea.StoreSelectedItem();
        }
    }

    protected override void Start()
    {
        base.Start();

        enabledCrosshair = false;
        drawing = false;
        weaponCameraAtChamberTransform = false;
        weaponCameraAtMagazineTransform = false;
    }

    void Update()
    {
        if (!UIController.Instance.CharacterMenu.IsVisible)
        {
            HandleMovement();
            HandleAim();
            HandleOperation();
            HandleCharacterUnarmedInput();
            HandleReload();
        }
        else
        {
            HandleCharacterMenuInput();
        }

        if (XboxOneInput.Instance.GetButtonUp(XboxOneButton.Menu))
        {
            UIController.Instance.CharacterMenu.Toggle();
            UIController.Instance.HUD.ReloadMenu.Hide();
        }
    }
}
