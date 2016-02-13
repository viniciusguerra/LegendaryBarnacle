using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
public class MagazineData : ItemData<Magazine>
{
    [XmlAttribute("Name")]
    public string itemName;

    [XmlElement("Capacity", typeof(int))]
    public int capacity;

    [XmlElement("Caliber")]
    public string caliber;    

    [XmlElement("Weight", typeof(float))]
    public float itemWeight;

    [XmlElement("Prefab")]
    public string prefabName;

    [SerializeField]
    private AmmoData currentAmmo;
    public AmmoData CurrentAmmo
    {
        get { return currentAmmo; }
        set { currentAmmo = value; }
    }

    [SerializeField]
    private int currentAmmoCount;
    public int CurrentAmmoCount
    {
        get { return currentAmmoCount; }
        set { currentAmmoCount = value; }
    }
    
    public int Capacity
    {
        get { return capacity; }
    }
    
    public string Caliber
    {
        get { return caliber; }
    }

    //TODO: Load rounds correctly (if changing ammo type, return current amount, etc.)
    public StackData[] Load(AmmoData ammo, int amount)
    {
        if (ammo.caliber != Caliber)
            return new StackData[]{ new StackData(ammo, amount) };

        StackData oldAmmoLeft = null;
        StackData newAmmoLeft = null;
        int previousAmmoCount = CurrentAmmoCount;

        if (CurrentAmmo != null && !string.IsNullOrEmpty(CurrentAmmo.ammoName) && ammo.ammoName != CurrentAmmo.ammoName)
        {
            oldAmmoLeft = new StackData(CurrentAmmo, currentAmmoCount);
            previousAmmoCount = 0;
        }        

        CurrentAmmo = ammo;
        CurrentAmmoCount = Mathf.Min(Capacity, previousAmmoCount + amount);

        int leftOverAmmo = amount - (CurrentAmmoCount - previousAmmoCount);

        if(leftOverAmmo > 0)
            newAmmoLeft = new StackData(ammo, leftOverAmmo);

        return new StackData[]{ oldAmmoLeft, newAmmoLeft };
    }

    public AmmoData Feed()
    {
        AmmoData ammoToReturn;

        if (CurrentAmmoCount > 0)
        {
            ammoToReturn = CurrentAmmo;

            CurrentAmmoCount--;

            if (CurrentAmmoCount == 0)
            {
                CurrentAmmo = null;
            }
        }
        else
        {
            ammoToReturn = null;
        }

        return ammoToReturn;
    }

    #region ItemData    
    public override string ItemName
    {
        get
        {
            return itemName;
        }
    }
    
    public override float Weight
    {
        get
        {
            return itemWeight;
        }
    }    

    public string PrefabName { get { return prefabName; } }

    [SerializeField]
    private MagazineDatabase magazineDatabase = new MagazineDatabase();
    public override ItemDatabase Database
    {
        get
        {
            return magazineDatabase;
        }
    }

    public Magazine CreateMagazine()
    {
        return Magazine.CreateMagazine(this);
    }
    #endregion
}
