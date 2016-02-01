using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Holster : Equipment
{
    [SerializeField]
    private HolsterData holsterData;

    public override ItemData ItemData
    {
        get { return holsterData; } protected set { holsterData = value as HolsterData; }
    }

    public float DrawTime
    {
        get
        {
            return holsterData.DrawTime;
        }
    }

    [SerializeField]
    private Firearm equippedFirearm;
    public Firearm EquippedFirearm
    {
        get
        {
            return equippedFirearm;
        }
    }    

    public Firearm Store(Firearm firearm)
    {
        Firearm oldFirearm = null;

        equippedFirearm = firearm;

        return oldFirearm;
    }

    public Firearm Draw(out float drawTime)
    {
        drawTime = holsterData.DrawTime;
        return equippedFirearm;
    }

    public Firearm Retrieve()
    {
        Firearm oldFirearm = equippedFirearm;

        equippedFirearm = null;
        
        return oldFirearm;
    }
}
