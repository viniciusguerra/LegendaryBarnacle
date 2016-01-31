using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ReloadableMagazineUI : MonoBehaviour/*, ISelectHandler, IDeselectHandler*/
{
    [SerializeField]
    private MagazineData magazineData;
    public MagazineData MagazineData
    {
        get { return magazineData; }
        set
        {
            magazineData = value;

            magazineNameText.text = MagazineData.ItemName;
            capacityText.text = MagazineData.CurrentAmmoCount.ToString() + "/" + MagazineData.Capacity.ToString();

            if (magazineData.CurrentAmmo != null)
            {
                ammoButton = BagItem.CreateButton(SceneManager.Instance.BagItemButtonPrefab, ammoValueTransform, magazineData.CurrentAmmo);
            }
            else
            {
                //if (ammoButton != null)
                //    Destroy(ammoButton);
            }           
        }
    }

    [SerializeField]
    private Text magazineNameText;    
    [SerializeField]
    private Text capacityText;
    [SerializeField]
    private Transform ammoValueTransform;
    [SerializeField]
    private BagItem ammoButton;
    [SerializeField]
    private float scaleTime = 0.2f;
    [SerializeField]
    private float selectedHeight = 95;
    [SerializeField]
    private float deselectedHeight = 40;
    private LayoutElement layoutElement;    

    public void OnSelect()
    {
        print("enter");

        iTween.Stop();
        iTween.ValueTo(gameObject, iTween.Hash("name", GetInstanceID() + "UpdateHeight", "onupdate", "UpdateHeight", "from", layoutElement.preferredHeight, "to", selectedHeight, "time", scaleTime));

        ammoButton.GetComponent<Button>().interactable = false;

        Navigation nav = ammoButton.GetComponent<Button>().navigation;
        nav.mode = Navigation.Mode.Vertical;
        ammoButton.GetComponent<Button>().navigation = nav;
    }

    public void OnDeselect()
    {
        print("exit");

        iTween.Stop();
        iTween.ValueTo(gameObject, iTween.Hash("name", GetInstanceID() + "UpdateHeight", "onupdate", "UpdateHeight", "from", layoutElement.preferredHeight, "to", deselectedHeight, "time", scaleTime));

        ammoButton.GetComponent<Button>().interactable = false;

        Navigation nav = ammoButton.GetComponent<Button>().navigation;
        nav.mode = Navigation.Mode.None;
        ammoButton.GetComponent<Button>().navigation = nav;
    }
    private void UpdateHeight(float height)
    {
        layoutElement.minHeight = height;
    }

    void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
    }

    void OnDestroy()
    {
        iTween.Stop();
    }

    //void ISelectHandler.OnSelect(BaseEventData eventData)
    //{
    //    OnSelect();
    //}

    //void IDeselectHandler.OnDeselect(BaseEventData eventData)
    //{
    //    OnDeselect();
    //}
}
