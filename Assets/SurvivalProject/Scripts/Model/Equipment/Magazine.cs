using UnityEngine;
using System.Collections;
using System;

public class Magazine : Equipment
{
    public Ammo currentAmmo;

    public int currentAmmoCount;
    public int maxAmmoCount;

    public int Load(Ammo ammo, int amount)
    {
        int previousAmmoCount = currentAmmoCount;

        currentAmmo = ammo;
        currentAmmoCount = Mathf.Min(maxAmmoCount, previousAmmoCount + amount);

        return amount - (currentAmmoCount - previousAmmoCount);
    }

    public Ammo Feed()
    {
        Ammo ammoToReturn;

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

    #region Collectible
    [SerializeField]
    private string collectibleName;
    public override string Name
    {
        get
        {
            return collectibleName;
        }
    }

    [SerializeField]
    private float collectibleWeight;
    public override float Weight
    {
        get
        {
            return collectibleWeight;
        }
    }
    #endregion
}
