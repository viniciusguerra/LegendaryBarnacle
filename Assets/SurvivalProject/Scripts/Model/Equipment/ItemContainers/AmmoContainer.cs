using UnityEngine;
using System.Collections;
using System;

public class AmmoContainer : ItemContainer
{
    [SerializeField]
    private AmmoData ammoData;
    public override ItemData ItemData
    {
        get
        {
            return ammoData;
        }
        protected set { ammoData = value as AmmoData; }
    }

    public AmmoData AmmoData
    {        
        set
        {
            ammoData = value;
        }
    }


    public string AmmoName { get { return ammoData.ItemName; } }
    public string Caliber { get { return ammoData.caliber; } }
    public float Damage { get { return ammoData.damage; } }
    public float Penetration { get { return ammoData.penetration; } }
    public float StoppingPower { get { return ammoData.stoppingPower; } }
    public float Weight { get { return ammoData.weight; } }
    public string PrefabName { get { return ammoData.prefabName; } }
}
