using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class VestData : ItemData
{
    [SerializeField]
    private int magazineCapacity;
    public int MagazineCapacity { get { return magazineCapacity; } }

    [SerializeField]
    private float defense;
    public float Defense
    {
        get { return defense; }
    }

    #region Collectible
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
