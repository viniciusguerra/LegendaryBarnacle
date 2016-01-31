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

    public void Initialize(ItemData itemData, int amount)
    {
        ItemContainer container;

        Type containerType = itemData.ContainerType.GetType();
        gameObject.AddComponent(containerType);            

        if (amount > 1)
        {
            gameObject.AddComponent<ItemStack>();
            container = GetComponent<ItemStack>();
            ((ItemStack)container).Amount = amount;
            ((ItemStack)container).StackData.ContainedItem = itemData;
        }
        else
        {
            container = GetComponent(containerType) as ItemContainer;
        }

        itemContainer = container;
    }

    public static void CreateCollectible(ItemData itemData, int amount, GameObject prefab, Vector3 position)
    {
        GameObject collectibleObject = Instantiate(prefab);

        collectibleObject.transform.position = position;
        collectibleObject.GetComponent<Collectible>().Initialize(itemData, amount);
    }

    public static void CreateCollectible(ItemData itemData, GameObject prefab, Vector3 position)
    {
        GameObject collectibleObject = Instantiate(prefab);

        collectibleObject.transform.position = position;
        collectibleObject.GetComponent<Collectible>().Initialize(itemData, 1);
    }
}
