using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

[Serializable]
public class AmmoInfoUI
{ 
    public Text caliber;
    public Text damage;
    public Text stoppingPower;
    public Text penetration;

    public IEnumerator Update(AmmoData ammoData)
    {
        while (UIController.Instance.CharacterMenu.InfoWindow.UpdateCoroutines)
        {
            caliber.text = ammoData.caliber;
            damage.text = ammoData.damage.ToString();
            stoppingPower.text = ammoData.stoppingPower.ToString();
            penetration.text = ammoData.penetration.ToString();

            yield return null;
        }
    }
}

[Serializable]
public class MagazineInfoUI
{
    public Text caliber;
    public Text capacity;
    public Transform ammoValueParent;

    public IEnumerator Update(MagazineData magazine, GameObject uiAmmoPrefab)
    {
        BagItem bagItem = BagItem.CreateToggle(uiAmmoPrefab, ammoValueParent, magazine.CurrentAmmo);

        UIController.Instance.CharacterMenu.InfoWindow.ammoUiObject = bagItem.gameObject;

        while (UIController.Instance.CharacterMenu.InfoWindow.UpdateCoroutines)
        {
            caliber.text = magazine.Caliber;
            capacity.text = magazine.CurrentAmmoCount.ToString() + '/' + magazine.Capacity.ToString();   

            yield return null;
        }
    }
}

[Serializable]
public class VestInfoUI
{
    public Text defense;
    public Text magazineCapacity;

    public IEnumerator Update(VestData vest)
    {
        while (UIController.Instance.CharacterMenu.InfoWindow.UpdateCoroutines)
        {
            defense.text = vest.Defense.ToString();
            magazineCapacity.text = vest.MagazineCount.ToString() + '/' + vest.MagazineCapacity.ToString();

            yield return null;
        }
    }
}

[Serializable]
public class FirearmInfoUI
{
    public Text type;
    public Text caliber;

    public IEnumerator Update(FirearmData firearm)
    {
        while (UIController.Instance.CharacterMenu.InfoWindow.UpdateCoroutines)
        {
            type.text = firearm.FirearmType;
            caliber.text = firearm.Caliber;

            yield return null;
        }
    }
}

[Serializable]
public class HolsterInfoUI
{
    public Text drawSpeed;

    public IEnumerator Update(HolsterData holster)
    {
        while (UIController.Instance.CharacterMenu.InfoWindow.UpdateCoroutines)
        {
            drawSpeed.text = holster.DrawTime.ToString() + 's';

            yield return null;
        }
    }
}

[Serializable]
public class ClothingInfoUI
{
    public Text defense;

    public IEnumerator Update(ClothingData clothing)
    {
        while (UIController.Instance.CharacterMenu.InfoWindow.UpdateCoroutines)
        {
            defense.text = clothing.Defense.ToString();

            yield return null;
        }
    }
}

public class InfoWindow : UIWindow
{
    [SerializeField]
    private UIWindow itemInfoWindow;
    [SerializeField]
    private Text itemName;
    [SerializeField]
    private Text itemAmount;
    [SerializeField]
    private Text itemWeight;
    [SerializeField]
    private Text itemDescription;
        
    [SerializeField]
    private AmmoInfoUI ammoInfo;
    [SerializeField]
    private MagazineInfoUI magazineInfo;
    [SerializeField]
    public MagazineEquipper magazineEquipper;
    [SerializeField]
    private VestInfoUI vestInfo;
    [SerializeField]
    private FirearmInfoUI firearmInfo;
    [SerializeField]
    private HolsterInfoUI holsterInfo;
    [SerializeField]
    private ClothingInfoUI clothingInfo;

    [SerializeField]
    private GameObject ammoUiPrefab;
    [HideInInspector]
    public GameObject ammoUiObject;

    [SerializeField]
    private GameObject ammoInfoGameObject;
    [SerializeField]
    private GameObject magazineInfoGameObject;
    [SerializeField]
    private GameObject vestInfoGameObject;
    [SerializeField]
    private GameObject firearmInfoGameObject;
    [SerializeField]
    private GameObject holsterInfoGameObject;
    [SerializeField]
    private GameObject clothingInfoGameObject;

    [SerializeField]
    private bool updateCoroutines;
    public bool UpdateCoroutines
    {
        get { return updateCoroutines; }
    }

    public void DisplayInfo(ItemData itemData)
    {
        SetInfoObjectsInactive();

        StackData stack = null;
        int amount = 1;

        if (itemData.GetType() == typeof(StackData))
            stack = itemData as StackData;        

        if (stack != null)
        {
            itemData = stack.ContainedItemData;
            amount = stack.Amount;
        }

        itemName.text = itemData.ItemName;
        itemAmount.text = amount > 1 ? "x" + amount.ToString() : string.Empty;
        itemWeight.text = itemData.Weight.ToString();
        //TODO: add description to ItemData

        itemInfoWindow.Show();
        updateCoroutines = true;

        switch(itemData.GetType().ToString())
        {
            case "AmmoData":
            {
                ammoInfoGameObject.SetActive(true);
                StartCoroutine(ammoInfo.Update(itemData as AmmoData));

                break;
            }
            case "MagazineData":
            {
                magazineInfoGameObject.SetActive(true);
                StartCoroutine(magazineInfo.Update(itemData as MagazineData, ammoUiPrefab));

                break;
            }
            case "VestData":
            {
                vestInfoGameObject.SetActive(true);
                StartCoroutine(vestInfo.Update(itemData as VestData));
                magazineEquipper.Initialize(itemData as VestData);

                break;
            }
            case "FirearmData":
            {
                firearmInfoGameObject.SetActive(true);
                StartCoroutine(firearmInfo.Update(itemData as FirearmData));

                break;
            }
            case "HolsterData":
            {
                holsterInfoGameObject.SetActive(true);
                StartCoroutine(holsterInfo.Update(itemData as HolsterData));

                break;
            }
            case "ClothingData":
            {
                clothingInfoGameObject.SetActive(true);
                StartCoroutine(clothingInfo.Update(itemData as ClothingData));

                break;
            }            
        }        
    }

    private void SetInfoObjectsInactive()
    {
        updateCoroutines = false;

        if (ammoUiObject != null)
            Destroy(ammoUiObject);
        
        ammoInfoGameObject.SetActive(false);
        magazineInfoGameObject.SetActive(false);
        vestInfoGameObject.SetActive(false);
        magazineEquipper.Hide();
        firearmInfoGameObject.SetActive(false);
        holsterInfoGameObject.SetActive(false);
        clothingInfoGameObject.SetActive(false);       
    }

    public override void Hide()
    {
        base.Hide();

        SetInfoObjectsInactive();

        itemInfoWindow.Hide();
    }
}
