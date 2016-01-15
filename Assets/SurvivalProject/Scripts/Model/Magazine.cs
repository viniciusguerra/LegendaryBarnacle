using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Magazine
{
    public Ammo currentAmmo;

    public int currentAmmoCount;
    public int maxAmmoCount;

    [ContextMenu("Load")]
    public void Load(Ammo ammo, int amount)
    {
        currentAmmo = ammo;
        currentAmmoCount = Mathf.Min(maxAmmoCount, currentAmmoCount + amount);
    }

    public void Feed(out Ammo chamber)
    {
        if(currentAmmoCount > 0)
        {
            chamber = currentAmmo;
            currentAmmoCount--;

            if(currentAmmoCount == 0)
            {
                currentAmmo = null;
            }
        }
        else
        {
            chamber = null;
        }
    }
}
