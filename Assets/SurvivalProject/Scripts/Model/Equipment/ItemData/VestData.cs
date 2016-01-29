using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class VestData : ItemData<Vest>
{
    [SerializeField]
    private int magazineCapacity;
    public int MagazineCapacity { get { return magazineCapacity; } }
    public int MagazineCount { get { return StoredMagazines.Length; } }

    [SerializeField]
    private float defense;
    public float Defense
    {
        get { return defense; }
    }

    [SerializeField]
    private Dictionary<Magazine, int> storedMagazines;
    public KeyValuePair<Magazine, int>[] StoredMagazines
    {
        get
        {
            if (storedMagazines == null)
                storedMagazines = new Dictionary<Magazine, int>();

            return storedMagazines.ToArray();
        }
    }

    #region Collectible
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
    private float itemWeight;
    public override float Weight
    {
        get
        {
            return itemWeight;
        }
    }

    public override ItemDatabase Database
    {
        get
        {
            //TODO: Create Vest Database
            throw new NotImplementedException();
        }
    }
    #endregion
}
