using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class MagazineUI : MonoBehaviour
{
    private MagazineData magazineData;
    public MagazineData MagazineData
    {
        get { return magazineData; }
        set
        {
            magazineData = value;

            if(ammoValue != null)
                DestroyImmediate(ammoValue);

            if(value != null)
            {
                if (value.CurrentAmmo != null && !string.IsNullOrEmpty(value.CurrentAmmo.ammoName))
                    ammoValue = BagItem.CreateButton(bagItemPrefab, ammoValueTransform, value.CurrentAmmo);
            }
        }
    }

    [SerializeField]
    private GameObject valuesGameObject;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text weightText;
    [SerializeField]
    private Text caliberText;
    [SerializeField]
    private Text capacityText;
    [SerializeField]
    private Transform ammoValueTransform;
    [SerializeField]
    private GameObject bagItemPrefab;

    private BagItem ammoValue;

    private bool updateInfo;
    public bool UpdateInfo
    {
        get { return updateInfo; }
        set
        {
            updateInfo = value;

            if(updateInfo == false)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Initialize(MagazineData magazine)
    {
        MagazineData = magazine;
                
        UpdateInfo = true;
    }

    public int LoadMagazine(AmmoData ammoData, int amount)
    {
        return magazineData.Load(ammoData, amount);
    }

    private void UpdateInfoValues()
    {
        if (magazineData != null)
        {
            valuesGameObject.SetActive(true);
            nameText.text = magazineData.ItemName;
            weightText.text = magazineData.Weight.ToString();

            caliberText.text = magazineData.Caliber;
            capacityText.text = magazineData.CurrentAmmoCount.ToString() + '/' + magazineData.Capacity.ToString();
        }
        else
        {
            valuesGameObject.SetActive(false);
            nameText.text = "Empty";
        }
    }

    void Update()
    {
        if (updateInfo)
            UpdateInfoValues();
    }
}
