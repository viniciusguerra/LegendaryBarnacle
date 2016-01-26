using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class MagazineData : ItemData
{
    [SerializeField]
    private string caliber;
    public string Caliber
    {
        get { return caliber; }
    }

    [SerializeField]
    private int capacity;
    public int Capacity
    {
        get { return capacity; }
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
