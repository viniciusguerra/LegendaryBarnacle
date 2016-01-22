﻿using UnityEngine;
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
public class AmmoData : ItemData
{
    [XmlAttribute("Name")]
    public string ammoName;
    [XmlElement("Caliber")]
    public string caliber;
    [XmlElement("Damage", typeof(float))]
    public float damage;
    [XmlElement("Penetration", typeof(float))]
    public float penetration;
    [XmlElement("StoppingPower", typeof(float))]
    public float stoppingPower;
    [XmlElement("Weight", typeof(float))]
    public float weight;
    [XmlElement("Prefab")]
    public string prefabName;

    public override string ItemName
    {
        get
        {
            return ammoName;
        }
    }

    public override float Weight
    {
        get
        {
            return weight;
        }
    }
}
