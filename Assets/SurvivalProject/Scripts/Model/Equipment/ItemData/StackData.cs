using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class StackData : ItemData
{
    [SerializeField]
    private ItemContainer containedItem;
    public ItemContainer ContainedItem
    {
        get { return containedItem; }
        set { containedItem = value; }
    }

    public ItemData ContainedItemData { get { return containedItem.ItemData; } }    

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
            return ContainedItemData.ItemName;
        }
    }

    public override float Weight
    {
        get
        {
            return ContainedItemData.Weight * amount;
        }
    }

    public override Type ContainerType
    {
        get
        {
            return typeof(ItemStack);
        }
    }
}
