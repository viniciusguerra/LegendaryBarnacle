using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Vest : Equipment
{
    [SerializeField]
    private int magazineCapacity;
    public int MagazineCapacity { get { return magazineCapacity; } }
    public int MagazineCount { get { return storedMagazines.Count; } }

    [SerializeField]
    private Dictionary<Magazine, int> storedMagazines;
    public KeyValuePair<Magazine, int>[] StoredMagazines
    {
        get { return storedMagazines.ToArray(); }
    }

    [SerializeField]
    private float defense;
    public float Defense
    {
        get { return defense; }
    }

    #region Collectible
    [SerializeField]
    private string collectibleName;
    public override string Name
    {
        get
        {
            return collectibleName;
        }
    }

    [SerializeField]
    private float collectibleWeight;
    public override float Weight
    {
        get
        {
            return collectibleWeight;
        }
    }
    #endregion

    void Start()
    {
        if(storedMagazines == null)
            storedMagazines = new Dictionary<Magazine, int>();
    }
}
