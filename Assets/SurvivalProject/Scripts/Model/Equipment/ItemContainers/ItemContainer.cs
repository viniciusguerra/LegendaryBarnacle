using UnityEngine;
using System.Collections;
using System;

public abstract class ItemContainer : MonoBehaviour
{
    public abstract ItemData ItemData { get; protected set; }

    public void InitializeItemData(ItemData value)
    {
        if (ItemData == null && ItemData.GetType().Equals(value.GetType()))
            ItemData = value;
    }
}
