using UnityEngine;
using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
public class Ammo
{
    [XmlAttribute("Name")]
    public string ammoName;
    [XmlElement("Caliber")]
    public string caliber;
    [XmlElement("Tip")]
    public string tip;
    [XmlElement("Damage", typeof(float))]
    public float damage;
    [XmlElement("Penetration", typeof(float))]
    public float penetration;
    [XmlElement("Prefab")]
    public string prefabName;    
}
