using UnityEngine;
using System.Collections;
using System;

public class Collectible : MonoBehaviour
{
    public static readonly string CollectibleTag = "Collectible";

    [SerializeField]
    private ItemContainer itemContainer;
    public ItemContainer ItemContainer { get { return itemContainer; } }
    public ItemData ItemData { get { return itemContainer.ItemData; } }

    public void Initialize(ItemData itemData)
    {
        Type containerType = itemData.ContainerType.GetType();
        gameObject.AddComponent(containerType);

        itemContainer = GetComponent(containerType) as ItemContainer;
    }

    public static void CreateCollectible(ItemData itemData, GameObject prefab, Vector3 position)
    {
        GameObject collectibleObject = Instantiate(prefab);

        collectibleObject.transform.position = position;
        collectibleObject.GetComponent<Collectible>().Initialize(itemData);
    }
}
