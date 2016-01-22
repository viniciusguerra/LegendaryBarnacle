﻿using UnityEngine;
using System.Collections;

public class Clothing : Equipment
{
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
}
