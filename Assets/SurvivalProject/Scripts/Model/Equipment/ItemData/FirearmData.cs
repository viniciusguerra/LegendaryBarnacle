using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class FirearmData : ItemData
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
    #endregion
}
