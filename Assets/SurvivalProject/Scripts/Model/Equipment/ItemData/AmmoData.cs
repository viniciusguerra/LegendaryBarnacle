using UnityEngine;
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
public class AmmoData : ItemData<AmmoContainer>, IDamageSource
{
    [XmlAttribute("Name")]
    public string ammoName;

    [XmlElement("Caliber")]
    public string caliber;

    [XmlElement("Damage", typeof(float))]
    public float damage;
    float IDamageSource.Damage
    {
        get
        {
            return damage;
        }
    }

    [XmlElement("Penetration", typeof(float))]
    public float penetration;

    [XmlElement("StoppingPower", typeof(float))]
    public float stoppingPower;
    float IDamageSource.Stagger
    {
        get
        {
            return stoppingPower;
        }
    }

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

    [SerializeField]
    private AmmoDatabase ammoDatabase = new AmmoDatabase();
    public override ItemDatabase Database
    {
        get
        {
            return ammoDatabase;
        }
    }
}
