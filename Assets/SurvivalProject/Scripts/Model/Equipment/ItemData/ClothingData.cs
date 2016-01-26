using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ClothingData : ItemData
{
    [SerializeField]
    private float defense;
    public float Defense
    {
        get { return defense; }
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
