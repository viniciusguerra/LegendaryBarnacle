using UnityEngine;
using System.Collections;
using System;

public class ItemStack : ItemContainer
{
    [SerializeField]
    private StackData stackData;

    public ItemData ContainedItemData
    {
        get { return stackData.ContainedItemData; }
    }

    public int Amount
    {
        get { return stackData.Amount; }
    }

    public override ItemData ItemData
    {
        get
        {
            return stackData;
        }
    }
}
