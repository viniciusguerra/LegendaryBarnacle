using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class Vest : Equipment
{
    [SerializeField]
    private VestData vestData;
    public override ItemData ItemData
    {
        get { return vestData; }
    }
    public VestData VestData
    {
        get { return vestData; }
    }

    public float Defense
    {
        get { return vestData.Defense; }
    }

    public int MagazineCapacity { get { return vestData.MagazineCapacity; } }

    public int MagazineCount { get { return vestData.MagazineCount; } }

    public KeyValuePair<Magazine, int>[] StoredMagazines
    {
        get { return vestData.StoredMagazines; }
    }
}
