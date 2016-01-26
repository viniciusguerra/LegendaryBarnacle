using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class AmmoInfoUI
{ 
    public Text caliber;
    public Text damage;
    public Text stoppingPower;
    public Text penetration;

    public IEnumerator Update(AmmoContainer ammoContainer)
    {
        while(true)
        {
            caliber.text = ammoContainer.Caliber;
            damage.text = ammoContainer.Damage.ToString();
            stoppingPower.text = ammoContainer.StoppingPower.ToString();
            penetration.text = ammoContainer.Penetration.ToString();

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

    public IEnumerator Update(Magazine magazine, GameObject uiAmmoPrefab)
    {
        uiAmmoPrefab.AddComponent<AmmoContainer>();
        uiAmmoPrefab.GetComponent<AmmoContainer>().AmmoData = magazine.currentAmmo;

        BagItem.Create(uiAmmoPrefab, ammoValueParent, uiAmmoPrefab.GetComponent<AmmoContainer>());

        while (true)
        {
            caliber.text = magazine.Caliber;
            capacity.text = magazine.currentAmmoCount.ToString() + '/' + magazine.Capacity.ToString();   

            yield return null;
        }
    }
}

[Serializable]
public class VestInfoUI
{
    public Text defense;
    public Text magazineCapacity;

    public IEnumerator Update(Vest vest)
    {
        while (true)
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

    public IEnumerator Update(Firearm firearm)
    {
        while (true)
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

    public IEnumerator Update(Holster holster)
    {
        while (true)
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

    public IEnumerator Update(Clothing clothing)
    {
        while (true)
        {
            defense.text = clothing.Defense.ToString();

            yield return null;
        }
    }
}

public class InfoWindow : UIWindow
{
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
    private VestInfoUI vestInfo;
    [SerializeField]
    private FirearmInfoUI firearmInfo;
    [SerializeField]
    private HolsterInfoUI holsterInfo;
    [SerializeField]
    private ClothingInfoUI clothingInfo;

    [SerializeField]
    private GameObject ammoUiPrefab;

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

    public void DisplayInfo(ItemContainer item)
    {
        SetInfoObjectsInactive();

        ItemStack stack = null;
        int amount = 1;

        if (item.GetType() == typeof(ItemStack))
            stack = (ItemStack)item;        

        if (stack != null)
        {
            item = stack.itemContainer;
            amount = stack.amount;
        }

        itemName.text = item.ItemData.ItemName;
        itemAmount.text = amount > 1 ? "x" + amount.ToString() : string.Empty;
        itemWeight.text = item.ItemData.Weight.ToString();
        //TODO: add description to ItemData

        switch(item.GetType().ToString())
        {
            case "AmmoContainer":
            {
                ammoInfoGameObject.SetActive(true);
                StartCoroutine(ammoInfo.Update(item as AmmoContainer));

                break;
            }
            case "Magazine":
            {
                magazineInfoGameObject.SetActive(true);
                StartCoroutine(magazineInfo.Update(item as Magazine, ammoUiPrefab));

                break;
            }
            case "Vest":
            {
                vestInfoGameObject.SetActive(true);
                StartCoroutine(vestInfo.Update(item as Vest));

                break;
            }
            case "Firearm":
            {
                firearmInfoGameObject.SetActive(true);
                StartCoroutine(firearmInfo.Update(item as Firearm));

                break;
            }
            case "Holster":
            {
                holsterInfoGameObject.SetActive(true);
                StartCoroutine(holsterInfo.Update(item as Holster));

                break;
            }
            case "Clothing":
            {
                clothingInfoGameObject.SetActive(true);
                StartCoroutine(clothingInfo.Update(item as Clothing));

                break;
            }            
        }        
    }

    private void SetInfoObjectsInactive()
    {
        StopAllCoroutines();
        ammoInfoGameObject.SetActive(false);
        magazineInfoGameObject.SetActive(false);
        vestInfoGameObject.SetActive(false);
        firearmInfoGameObject.SetActive(false);
        holsterInfoGameObject.SetActive(false);
        clothingInfoGameObject.SetActive(false);
    }

    public override void Hide()
    {
        base.Hide();

        SetInfoObjectsInactive();
    }
}
