using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;

[XmlRoot("AmmoCollection")]
public class AmmoDatabase
{
    [XmlArray("AmmoArray")]
    [XmlArrayItem("Ammo")]
    public List<AmmoData> ammoList = new List<AmmoData>();

    public static readonly string ammoDatabasePath = "Assets/SurvivalProject/Database/Ammo.xml";
    public static readonly string ammoPrefabsPath = "Assets/SurvivalProject/Prefabs/Ammo/";

    public static List<AmmoData> Load()
    {
        TextAsset xml = AssetDatabase.LoadAssetAtPath<TextAsset>(ammoDatabasePath);

        XmlSerializer serializer = new XmlSerializer(typeof(AmmoDatabase));

        StringReader reader = new StringReader(xml.text);

        AmmoDatabase ammoDatabase = serializer.Deserialize(reader) as AmmoDatabase;

        reader.Close();

        return ammoDatabase.ammoList;
    }

    public static string[] LoadNames()
    {
        List<AmmoData> ammoInDatabase = Load();
        List<string> ammoNamesInDatabase = new List<string>();

        ammoInDatabase.ForEach(ammo => ammoNamesInDatabase.Add(ammo.ammoName));

        return ammoNamesInDatabase.ToArray();
    }

    public static AmmoData LoadByName(string name)
    {
        AmmoData ammo = Load().Find(x => x.ammoName == name);

        if (ammo == null)
        {
            throw new UnityException("No ammo with given name on Database");
        }

        return ammo;
    }
}
