using UnityEngine;
using System.Collections;
using System;

public class ItemStack : ItemContainer
{
    public ItemContainer container;

    [SerializeField]
    private StackData stackData;
    public StackData StackData { get { return StackData; } }
    public ItemData ContainedItemData
    {
        get { return stackData.ContainedItem; }
    }

    public int Amount
    {
        get { return stackData.Amount; }
        set { stackData.Amount = value;}
    }

    public override ItemData ItemData
    {
        get
        {
            return stackData;
        }
        protected set { stackData = value as StackData; }
    }

    void Awake()
    {
        stackData.ContainedItem = container.ItemData;
    }
}
