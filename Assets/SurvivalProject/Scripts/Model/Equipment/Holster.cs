using UnityEngine;
using System.Collections;
using System;

public class Holster : Equipment
{
    [SerializeField]
    private Firearm equippedFirearm;
    public Firearm EquippedFirearm
    {
        get
        {
            return equippedFirearm;
        }
    }

    [SerializeField]
    private float drawTime;
    public float DrawTime
    {
        get
        {
            return drawTime;
        }
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

    public Firearm Store(Firearm firearm)
    {
        Firearm oldFirearm = null;

        equippedFirearm = firearm;

        return oldFirearm;
    }

    public Firearm Draw(out float drawTime)
    {
        drawTime = this.drawTime;
        return equippedFirearm;
    }

    public Firearm Retrieve()
    {
        Firearm oldFirearm = equippedFirearm;

        equippedFirearm = null;
        
        return oldFirearm;
    }
}
