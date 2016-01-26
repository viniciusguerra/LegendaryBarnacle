using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HolsterData : ItemData
{
    [SerializeField]
    private float drawTime;
    public float DrawTime
    {
        get
        {
            return drawTime;
        }
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
