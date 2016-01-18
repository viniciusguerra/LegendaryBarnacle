using UnityEngine;
using System.Collections;
using System;

public interface ICollectible
{
    string Name { get; }
    float Weight { get; }
}

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private ICollectible collectible;

    [SerializeField]
    private int amount;

    public static void CreateCollectible(ICollectible collectible, int amount, Vector3 position)
    {
        //instantiate collectible prefab from SceneManager
        throw new NotImplementedException();
    }
}
