using UnityEngine;
using System.Collections;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField]
    private CustomCamera mainCamera;
    [SerializeField]
    private Camera weaponCamera;
    public float weaponCameraTranslateTime = 0.5f;

    [SerializeField]
    private GameObject collectiblePrefab;
    public GameObject CollectiblePrefab { get { return collectiblePrefab; } }

    [SerializeField]
    private GameObject bagItemButtonPrefab;
    public GameObject BagItemButtonPrefab { get { return bagItemButtonPrefab; } }

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

        Vector3 localPosition = weaponCamera.transform.parent.InverseTransformPoint(targetTransform.position);

        iTween.MoveTo(weaponCamera.gameObject, iTween.Hash("position", localPosition, "time", instant ? 0 : weaponCameraTranslateTime, "islocal", true, "easetype", iTween.EaseType.easeOutQuad));
        iTween.RotateTo(weaponCamera.gameObject, iTween.Hash("rotation", targetTransform, "time", instant ? 0 : weaponCameraTranslateTime, "easetype", iTween.EaseType.easeOutQuad));
    }
}
