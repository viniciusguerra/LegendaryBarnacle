using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class BagData : ItemData<Bag>
{
    [SerializeField]
    private float maxWeight;
    public float MaxWeight { get { return maxWeight; } }

    [SerializeField]
    private float currentWeight;
    public float CurrentWeight { get { return currentWeight; } set { currentWeight = value; } }

    [SerializeField]
    private List<ItemData> storedItems;
    public List<ItemData> StoredItems
    {
        get { return storedItems; }
        set { storedItems = value; }
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
            //TODO: Create Bag Database
            throw new NotImplementedException();
        }
    }
    #endregion
}
