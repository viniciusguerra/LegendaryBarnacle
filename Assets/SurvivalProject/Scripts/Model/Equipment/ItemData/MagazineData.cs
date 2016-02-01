using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class MagazineData : ItemData<Magazine>
{
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

    [SerializeField]
    private int capacity;
    public int Capacity
    {
        get { return capacity; }
    }

    [SerializeField]
    private string caliber;
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

        if (!string.IsNullOrEmpty(CurrentAmmo.ammoName) && ammo.ammoName != CurrentAmmo.ammoName)
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
    [SerializeField]
    private string itemName;
    public override string ItemName
    {
        get
        {
            return itemName;
        }
    }

    [SerializeField]
    private float itemWeight;
    public override float Weight
    {
        get
        {
            return itemWeight;
        }
    }

    public override ItemDatabase Database
    {
        get
        {
            //TODO: Create Magazine Database
            throw new NotImplementedException();
        }
    }
    #endregion
}
