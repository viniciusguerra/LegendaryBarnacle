using UnityEngine;
using System.Collections;
using System;

public class ItemStack : ItemContainer
{
    public ItemContainer itemContainer;

    public int amount;

    public override ItemData ItemData
    {
        get
        {
            return itemContainer.ItemData;
        }
    }
}
