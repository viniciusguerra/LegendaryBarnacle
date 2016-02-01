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
    public int MagazineCount { get { return StoredMagazines.Count; } }

    [SerializeField]
    private float defense;
    public float Defense
    {
        get { return defense; }
    }

    [SerializeField]
    private List<MagazineData> storedMagazines;
    public List<MagazineData> StoredMagazines
    {
        get
        {
            if (storedMagazines == null)
                storedMagazines = new List<MagazineData>();

            return storedMagazines;
        }
    }

    public MagazineData StoreMagazine(MagazineData magazineData)
    {
        if (StoredMagazines.Count < MagazineCapacity)
        {
            StoredMagazines.Add(magazineData);
            return null;
        }
        else
            return magazineData;
    }

    public MagazineData RetrieveMagazine(MagazineData magazineData)
    {
        int? magazineIndex = StoredMagazines.FindIndex(x => x == magazineData);

        if (magazineIndex.HasValue)
        {
            StoredMagazines.RemoveAt(magazineIndex.Value);
            return magazineData;
        }
        else
            return null;
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
