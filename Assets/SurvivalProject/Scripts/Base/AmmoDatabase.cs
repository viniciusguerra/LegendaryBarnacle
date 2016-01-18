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
    public List<Ammo> ammoList = new List<Ammo>();

    public static readonly string ammoDatabasePath = "Assets/SurvivalProject/Database/Ammo.xml";
    public static readonly string ammoPrefabsPath = "Assets/SurvivalProject/Prefabs/Ammo/";

    public static List<Ammo> Load()
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
        List<Ammo> ammoInDatabase = Load();
        List<string> ammoNamesInDatabase = new List<string>();

        ammoInDatabase.ForEach(ammo => ammoNamesInDatabase.Add(ammo.ammoName));

        return ammoNamesInDatabase.ToArray();
    }

    public static Ammo LoadByName(string name)
    {
        Ammo ammo = Load().Find(x => x.ammoName == name);

        if (ammo == null)
        {
            throw new UnityException("No ammo with given name on Database");
        }

        return ammo;
    }
}
