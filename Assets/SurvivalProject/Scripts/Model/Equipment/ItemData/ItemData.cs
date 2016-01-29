using UnityEngine;
using System.Collections;
using System;

public abstract class ItemData
{
    public abstract Type ContainerType
    {
        get;
    }  

    public abstract string ItemName
    {
        get;        
    }

    public abstract float Weight
    {
        get;
    }
}

public abstract class ItemData<T> : ItemData where T : ItemContainer
{
    public sealed override Type ContainerType
    {
        get
        {
            return typeof(T);
        }
    }

    public abstract ItemDatabase Database { get; }
}
