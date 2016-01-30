using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Magazine : Equipment
{
    [SerializeField]
    private MagazineData magazineData;
    public MagazineData MagazineData
    {
        get { return magazineData; }
    }

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
}
