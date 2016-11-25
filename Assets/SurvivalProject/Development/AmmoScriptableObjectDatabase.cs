using UnityEngine;
using System.Collections.Generic;

public class AmmoScriptableObjectDatabase : ScriptableObject
{
    [SerializeField]
    private List<AmmoData> ammoDataList;
    public List<AmmoData> AmmoDataList
    {
        get
        {
            return this.ammoDataList;
        }
        set
        {
            ammoDataList = value;
        }
    }
}
