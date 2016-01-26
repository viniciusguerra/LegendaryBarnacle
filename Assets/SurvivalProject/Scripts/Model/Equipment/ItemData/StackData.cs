using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class StackData : ItemData
{
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
    private float weight;
    public override float Weight
    {
        get
        {
            return weight;
        }
    }

    public void SetWeight(float value)
    {
        weight = value;
    }

    public void SetName(string value)
    {
        itemName = value;
    }
}
