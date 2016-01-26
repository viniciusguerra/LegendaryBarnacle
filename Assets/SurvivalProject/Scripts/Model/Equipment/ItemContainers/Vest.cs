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

    public int MagazineCount { get { return storedMagazines.Count; } }

    [SerializeField]
    private Dictionary<Magazine, int> storedMagazines;
    public KeyValuePair<Magazine, int>[] StoredMagazines
    {
        get { return storedMagazines.ToArray(); }
    }    

    void Start()
    {
        if(storedMagazines == null)
            storedMagazines = new Dictionary<Magazine, int>();
    }
}
