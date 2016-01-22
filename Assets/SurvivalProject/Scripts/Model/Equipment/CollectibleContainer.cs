using UnityEngine;
using System.Collections;

public class CollectibleContainer : MonoBehaviour, ICollectible
{
    #region Collectible
    [SerializeField]
    private string collectibleName;
    public string Name
    {
        get
        {
            return collectibleName;
        }
    }

    [SerializeField]
    private float collectibleWeight;
    public float Weight
    {
        get
        {
            return collectibleWeight;
        }
    }
    #endregion
}
