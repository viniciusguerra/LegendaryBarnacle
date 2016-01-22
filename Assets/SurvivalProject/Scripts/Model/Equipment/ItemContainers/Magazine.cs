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

    public AmmoData currentAmmo;

    public int currentAmmoCount;

    public int Load(AmmoData ammo, int amount)
    {
        if (ammo.caliber != magazineData.Caliber)
            return amount;

        int previousAmmoCount = currentAmmoCount;

        currentAmmo = ammo;
        currentAmmoCount = Mathf.Min(magazineData.Capacity, previousAmmoCount + amount);

        return amount - (currentAmmoCount - previousAmmoCount);
    }

    public AmmoData Feed()
    {
        AmmoData ammoToReturn;

        if(currentAmmoCount > 0)
        {
            ammoToReturn = currentAmmo;

            currentAmmoCount--;

            if(currentAmmoCount == 0)
            {
                currentAmmo = null;
            }            
        }
        else
        {
            ammoToReturn = null;
        }

        return ammoToReturn;
    }    
}
