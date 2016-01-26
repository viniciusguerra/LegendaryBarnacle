using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BagData : ItemData
{
    [SerializeField]
    private float maxWeight;
    public float MaxWeight { get { return maxWeight; } }

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
