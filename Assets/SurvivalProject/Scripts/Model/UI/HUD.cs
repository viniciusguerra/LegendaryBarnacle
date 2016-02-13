using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private ReloadMenu reloadMenu;
    public ReloadMenu ReloadMenu
    {
        get { return reloadMenu; }
    }

    public UIWindow weaponCamera;

    public void ToggleWeaponCamera()
    {
        weaponCamera.Toggle();
    }

    public void SetWeaponCameraVisibility(bool visible)
    {
        if (visible)
            weaponCamera.Show();
        else
            weaponCamera.Hide();
    }

    void Start()
    {

    }
}
