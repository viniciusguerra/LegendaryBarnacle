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
    public static readonly string CollectibleTag = "Collectible";

    //[SerializeField]
    //private CollectibleContainer container;

    [SerializeField]
    private ICollectible collectible;

    [SerializeField]
    private int amount;

    void Awake()
    {
        collectible = GetComponent<ICollectible>();
    }

    public void Initialize(ICollectible collectible, int amount)
    {
        this.collectible = collectible;
        this.amount = amount;
    }

    public static void CreateCollectible(ICollectible collectible, int amount, GameObject prefab, Vector3 position)
    {
        GameObject collectibleObject = Instantiate(prefab);

        collectibleObject.transform.position = position;
        collectibleObject.GetComponent<Collectible>().Initialize(collectible, amount);
    }
}
