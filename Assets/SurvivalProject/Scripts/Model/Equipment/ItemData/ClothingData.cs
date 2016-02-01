using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ClothingData : ItemData<Clothing>
{
    [SerializeField]
    private float defense;
    public float Defense
    {
        get { return defense; }
    }

    #region ItemData
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
            //TODO: Create Clothing Database
            throw new NotImplementedException();
        }
    }
    #endregion
}
