using UnityEngine;
using System.Collections;
using System;

public abstract class Equipment : MonoBehaviour, ICollectible
{
    public abstract string Name { get; }
    public abstract float Weight { get; }
}
