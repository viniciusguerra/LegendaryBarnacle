using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Clothing : Equipment
{
    [SerializeField]
    private ClothingData clothingData;
    public override ItemData ItemData { get { return clothingData; } }

    public float Defense
    {
        get { return clothingData.Defense; }
    }
}
