using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEditor;
using System;
using System.Reflection;

public abstract class ItemDatabase : UnityEngine.Object
{
    public abstract string DatabasePath { get; }
    public abstract string PrefabsPath { get; }
}

public abstract class ItemDatabase<T> : ItemDatabase where T : ItemData
{
    //Properties without set accessor are not XML serialized
    public abstract List<T> ItemList { get; protected set; }

    /// <summary>
    /// Loads all data from the database
    /// </summary>
    /// <returns>An array containing all the entries from the database</returns>
    //public abstract List<T> Load(); FUCKING WORKS
    public List<T> Load()
    {
        TextAsset xml = AssetDatabase.LoadAssetAtPath<TextAsset>(DatabasePath);

        StringReader reader = new StringReader(xml.text);

        XmlSerializer serializer = new XmlSerializer(typeof(AmmoDatabase));

        ItemDatabase<T> database = serializer.Deserialize(reader) as ItemDatabase<T>;

        ItemList = database.ItemList;

        reader.Close();

        return ItemList;
    }

    /// <summary>
    /// Loads values of a given property from all objects on the database
    /// </summary>
    /// <param name="property">The property to be loaded</param>
    /// <returns>Array containing values of the property from the database</returns>
    public Array LoadPropertyValues(PropertyInfo property)
    {
        List<object> propertyValues = new List<object>();
        List<T> databaseItems = Load();

        if (property.DeclaringType == typeof(T))
        {
            databaseItems.ForEach(data => propertyValues.Add(property.GetValue(data, null)));
        }
        else
            throw new UnityException("Property does not belong to " + typeof(T).Name + " Type");

        return propertyValues.ToArray();
    }

    /// <summary>
    /// Returns an object from the database that meets the given condition
    /// </summary>
    /// <param name="condition">A predicate for finding the object</param>
    /// <returns>The first object for which the given condition is true</returns>
    public T Find(Predicate<T> condition)
    {
        T data = Load().Find(condition);

        if (data == null)
        {
            throw new UnityException("No object on database meets the given condition");
        }

        return data;
    }
}

[XmlRoot("AmmoCollection")]
public class AmmoDatabase : ItemDatabase<AmmoData>
{
    protected List<AmmoData> ammoList;

    [XmlArray("AmmoArray")]
    [XmlArrayItem("Ammo")]
    public override List<AmmoData> ItemList
    {
        get
        {
            return ammoList;
        }
        protected set
        {
            ammoList = value;
        }
    }

    public override string DatabasePath { get { return Constants.AmmoDatabasePath; } }
    public override string PrefabsPath { get { return Constants.AmmoPrefabsPath; } }
}


