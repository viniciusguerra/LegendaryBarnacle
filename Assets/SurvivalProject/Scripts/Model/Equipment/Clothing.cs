using UnityEngine;
using System.Collections;

public class Clothing : Equipment
{
    [SerializeField]
    private float defence;
    public float Defence
    {
        get { return defence; }
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
}
