using UnityEngine;
using System.Collections;
using System;

public class ItemStack : ItemContainer
{
    public ItemContainer itemContainer;
    [SerializeField]
    private StackData stackData;

    public int amount;

    public override ItemData ItemData
    {
        get
        {
            stackData.SetName(itemContainer.ItemData.ItemName);
            stackData.SetWeight(itemContainer.ItemData.Weight * amount);

            return stackData;
        }
    }

    void Awake()
    {
        
    }
}
