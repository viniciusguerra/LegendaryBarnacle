using UnityEngine;
using System.Collections;

public enum MovementState
{
    Walking,
    Running
}

//[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CustomCharacterController))]
public class PlayerInput : CharacterInput
{
    [SerializeField]
    private float horizontalInputLerpStep = 0.1f;
    [SerializeField]
    private float forwardInputLerpStep = 0.1f;

    [SerializeField]
    private new CustomCamera camera;
    private Vector3 cameraForward;             // The current forward direction of the camera
    private Vector3 movementInput;
    float horizontalInput, forwardInput;

    protected bool operationEnabled;
    protected bool holdingTrigger;
    protected bool weaponCameraAtChamberTransform;
    protected bool weaponCameraAtMagazineTransform;

    [SerializeField]
    private float buttonHoldTime = 0.2f;

    //[SerializeField]
    //private LootArea lootArea;

    protected void HandleMovement()
    {
        //lastPosition = transform.position;

        //float horizontalMovementAxis = XboxOneInput.Instance.GetAxis(XboxOneAxis.LeftThumbX);
        //float verticalMovementAxis = XboxOneInput.Instance.GetAxis(XboxOneAxis.LeftThumbY);
        //bool hasMovementInput = horizontalMovementAxis != 0 || verticalMovementAxis != 0;

        //Vector3 movementDirection = new Vector3(horizontalMovementAxis, 0, verticalMovementAxis) * currentSpeed * Time.deltaTime;

        //if (hasMovementInput)
        //    Move(movementDirection);

        // read inputs
        horizontalInput = Mathf.Lerp(XboxOneInput.Instance.GetAxis(XboxOneAxis.LeftThumbX), horizontalInput, horizontalInputLerpStep);
        forwardInput = Mathf.Lerp(XboxOneInput.Instance.GetAxis(XboxOneAxis.LeftThumbY), forwardInput, forwardInputLerpStep);

        // calculate move direction to pass to character
        if (camera != null)
        {
            // calculate camera relative direction to move:
            cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            movementInput = forwardInput * cameraForward + horizontalInput * camera.transform.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            movementInput = forwardInput * Vector3.forward + horizontalInput * Vector3.right;
        }
        // pass all parameters to the character control script
        if (CharacterController.IsAiming)
        {
            MoveStrafing(movementInput);
        }
        else
        {
            MoveTurning(movementInput);
        }
    }

    protected void HandleAim()
    {
        //toggle aiming
        if (XboxOneInput.Instance.GetButton(XboxOneButton.LB))
        {
            //holster weapon
            if (characterController.IsAiming)
            {
                if (characterController.IsAiming && !CharacterController.IsHolstering)
                {
                    operationEnabled = false;

                    characterController.HolsterWeapon();
                }
            }
            else
            {
                //start drawing
                if (!characterController.IsDrawing && !characterController.IsAiming)
                {
                    characterController.DrawWeapon();                    
                }  
            }
        }

        if(characterController.IsDrawing && Character.WieldedFirearm != null)
            operationEnabled = true;

        if (characterController.IsAiming)
        {
            var horizontalInput = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbX);
            var verticalInput = XboxOneInput.Instance.GetAxis(XboxOneAxis.RightThumbY);

            float targetHorizontalAngle = horizontalInput;
            float targetVerticalAngle = verticalInput;

            // Rotate the rig (the root object) around Y axis only:
            Quaternion targetWeaponRotation = Quaternion.Euler(-targetVerticalAngle, targetHorizontalAngle, 0f);

            characterController.SetAimRotation(targetWeaponRotation);            
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
            {
                XboxOneInput.Instance.OnButtonHeld(XboxOneButton.Y, buttonHoldTime, Character.WieldedFirearm.HalfSlideToggle, Character.WieldedFirearm.FullSlidePull);
            }

            //Set Weapon Camera to check slide
            //if (weaponCameraAtChamberTransform == false && Character.WieldedFirearm.SlideHalfBack)
            //{
            //    SceneManager.Instance.SetWeaponCameraTransform(Character.WieldedFirearm.ChamberTransform, false);
            //    weaponCameraAtMagazineTransform = false;
            //    weaponCameraAtChamberTransform = true;
            //}

            //release slide
            //will load chamber if magazine is loaded
            if (XboxOneInput.Instance.GetButtonUp(XboxOneButton.Y))
            {
                //SceneManager.Instance.SetWeaponCameraTransform(weaponCameraOperationTransform, false);
                Character.WieldedFirearm.ReleaseSlide();
            }

            //Set Weapon Camera back if slide is released
            //if (weaponCameraAtChamberTransform == true && (!Character.WieldedFirearm.SlideHalfBack && !Character.WieldedFirearm.SlideFullBack))
            //{
            //    SceneManager.Instance.SetWeaponCameraTransform(Character.WieldedFirearm.OperationTransform, false);
            //    weaponCameraAtChamberTransform = false;
            //}

            if (characterController.IsAiming)
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
        //loot area controls
        //if (!operationEnabled && !characterController.IsAiming)
        //{
        //    if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.Y))
        //        lootArea.CycleItems();

        //    if (XboxOneInput.Instance.GetButtonDown(XboxOneButton.A))
        //        lootArea.StoreSelectedItem();
        //}
    }

    protected override void Start()
    {
        base.Start();

        //enabledCrosshair = false;
        //drawing = false;
        weaponCameraAtChamberTransform = false;
        weaponCameraAtMagazineTransform = false;

        characterController = GetComponent<CustomCharacterController>();
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
