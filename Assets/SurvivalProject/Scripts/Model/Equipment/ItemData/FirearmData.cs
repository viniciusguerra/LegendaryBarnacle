using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FirearmData : ItemData<Firearm>
{
    [SerializeField]
    private string firearmType;
    public string FirearmType
    {
        get { return firearmType; }
    }

    [SerializeField]
    private string caliber;
    public string Caliber
    {
        get { return caliber; }
    }

    [SerializeField]
    protected AmmoData chamberedAmmo;
    public AmmoData ChamberedAmmo
    {
        get { return chamberedAmmo; }
        set { if (value == null || value.caliber == caliber) chamberedAmmo = value; }
    }

    [SerializeField]
    private Magazine currentMagazine;
    public Magazine CurrentMagazine
    {
        get { return currentMagazine; }
        set { if (currentMagazine.Caliber == Caliber || value == null) currentMagazine = value; }
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
            //TODO: Create Firearm Database
            throw new NotImplementedException();
        }
    }
    #endregion
}
