using UnityEngine;
using System.Collections;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField]
    private CustomCamera mainCamera;
    [SerializeField]
    private Camera weaponCamera;
    public float weaponCameraTranslateTime = 0.5f;

    public CustomCamera MainCamera
    {
        get
        {
            return mainCamera;
        }
    }

    public void SetWeaponCameraTransform(Transform targetTransform, bool instant)
    {
        iTween.Stop(weaponCamera.gameObject);

        iTween.MoveTo(weaponCamera.gameObject, iTween.Hash("position", targetTransform.localPosition, "time", instant ? 0 : weaponCameraTranslateTime, "islocal", true));
        iTween.RotateTo(weaponCamera.gameObject, iTween.Hash("rotation", targetTransform.localRotation.eulerAngles, "time", instant ? 0 : weaponCameraTranslateTime, "islocal", true));
    }
}
