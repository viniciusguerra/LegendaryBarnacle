using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Magazine : Equipment
{
    [SerializeField]
    private MagazineData magazineData;

    public override ItemData ItemData
    {
        get { return magazineData; }
    }

    public string Caliber { get { return magazineData.Caliber; } }
    public int Capacity { get { return magazineData.Capacity; } }

    public AmmoData CurrentAmmo
    {
        get
        {
            return magazineData.CurrentAmmo;
        }
        set
        {
            magazineData.CurrentAmmo = value;
        }
    }

    public int CurrentAmmoCount
    {
        get
        {
            return magazineData.CurrentAmmoCount;
        }
        set
        {
            magazineData.CurrentAmmoCount = value;
        }
    }

    public int Load(AmmoData ammo, int amount)
    {
        if (ammo.caliber != magazineData.Caliber)
            return amount;

        int previousAmmoCount = CurrentAmmoCount;

        CurrentAmmo = ammo;
        CurrentAmmoCount = Mathf.Min(magazineData.Capacity, previousAmmoCount + amount);

        return amount - (CurrentAmmoCount - previousAmmoCount);
    }

    public AmmoData Feed()
    {
        AmmoData ammoToReturn;

        if(CurrentAmmoCount > 0)
        {
            ammoToReturn = CurrentAmmo;

            CurrentAmmoCount--;

            if(CurrentAmmoCount == 0)
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
}
