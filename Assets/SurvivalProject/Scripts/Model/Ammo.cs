using UnityEngine;
using System.Collections;

public enum AmmoCaliber
{    
    _38,
    _9mm,
    _45acp,
}

public enum AmmoTip
{
    FullMetalJacket,
    HollowPoint,
    Piercing
}

public class Ammo : ScriptableObject
{
    public AmmoCaliber caliber;
    public AmmoTip tip;
}
