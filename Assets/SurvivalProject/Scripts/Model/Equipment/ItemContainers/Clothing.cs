using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Clothing : Equipment
{
    [SerializeField]
    private ClothingData clothingData;
    public override ItemData ItemData { get { return clothingData; } protected set { clothingData = value as ClothingData; } }

    public float Defense
    {
        get { return clothingData.Defense; }
    }
}
