using UnityEngine;
using System.Collections;
using System;

public class Collectible : MonoBehaviour
{
    public static readonly string CollectibleTag = "Collectible";

    [SerializeField]
    private ItemContainer item;
    public ItemContainer Item { get { return item; } }

    public void Initialize(ItemContainer item)
    {
        this.item = item;
    }

    public static void CreateCollectible(ItemContainer item, GameObject prefab, Vector3 position)
    {
        GameObject collectibleObject = Instantiate(prefab);

        collectibleObject.transform.position = position;
        collectibleObject.GetComponent<Collectible>().Initialize(item);
    }
}
