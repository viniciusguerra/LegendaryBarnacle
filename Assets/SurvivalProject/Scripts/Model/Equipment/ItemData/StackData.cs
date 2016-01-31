using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class StackData : ItemData
{
    [SerializeField]
    private ItemData containedItem;
    public ItemData ContainedItem { get { return containedItem; } set { containedItem = value; } }    

    [SerializeField]
    private int amount;
    public int Amount { get { return amount; }
        set
        {
            amount = value;
        }
    }

    public override string ItemName
    {
        get
        {
            return containedItem.ItemName;
        }
    }

    public override float Weight
    {
        get
        {
            return containedItem.Weight * amount;
        }
    }

    public override Type ContainerType
    {
        get
        {
            return typeof(ItemStack);
        }
    }

    public StackData(ItemData itemData, int amount)
    {
        containedItem = itemData;
        this.amount = amount;
    }
}
