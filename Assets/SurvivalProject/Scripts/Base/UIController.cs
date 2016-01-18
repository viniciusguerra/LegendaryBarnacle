using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : Singleton<UIController>
{
    public RawImage weaponCamera;
    public float weaponCameraFadeTime = 1f;
    private bool weaponCameraVisible = false;

    public void ToggleWeaponCamera()
    {
        weaponCameraVisible = !weaponCameraVisible;

        Color color = weaponCamera.color;
        Color newColor = color;
        newColor.a = weaponCameraVisible ? 1 : 0;

        iTween.ValueTo(gameObject, iTween.Hash("from", color, "to", newColor, "time", weaponCameraFadeTime, "onupdate", "UpdateWeaponCameraAlpha"));
    }

    public void SetWeaponCamera(bool visible)
    {
        weaponCameraVisible = visible;

        Color color = weaponCamera.color;
        Color newColor = color;
        newColor.a = weaponCameraVisible ? 1 : 0;

        iTween.ValueTo(gameObject, iTween.Hash("from", color, "to", newColor, "time", weaponCameraFadeTime, "onupdate", "UpdateWeaponCameraAlpha"));
    }

    private void UpdateWeaponCameraAlpha(Color value)
    {
        weaponCamera.color = value;
    }

    void Start()
    {
        //Hiding Weapon Camera
        Color color = weaponCamera.color;
        color.a = 0;
        weaponCamera.color = color;
    }
}
