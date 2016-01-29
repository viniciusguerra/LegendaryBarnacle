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
