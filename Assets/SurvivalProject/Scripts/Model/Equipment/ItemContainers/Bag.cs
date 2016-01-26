using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class Bag : Equipment
{
    [SerializeField]
    private BagData bagData;

    public override ItemData ItemData
    {
        get { return bagData; }
    }

    public float MaxWeight { get { return bagData.MaxWeight; } }

    [SerializeField]
    private float currentWeight;
    public float CurrentWeight { get { return currentWeight; } }

    [SerializeField]
    private Dictionary<ItemContainer, int> storedItems;
    public KeyValuePair<ItemContainer, int>[] StoredItems
    {
        get { return storedItems.ToArray(); }
    }

    /// <summary>
    /// Stores a single unit of the given collectible to bag and updates its current weight. Returns if it was stored or not.
    /// </summary>
    /// <param name="itemContainer">Item to be stored</param>
    /// <returns>True if it was stored</returns>
    public bool Store(ItemContainer itemContainer)
    {
        if(bagData.MaxWeight >= currentWeight + itemContainer.ItemData.Weight)
        {
            if (!storedItems.ContainsKey(itemContainer))            
                storedItems.Add(itemContainer, 1);
            else
                storedItems[itemContainer]++;

            itemContainer.transform.parent = transform;
            ReparentItemContainer(itemContainer);
            currentWeight += itemContainer.ItemData.Weight;
            return true;
        }
        else
        {
            return false;
        }        
    }

    /// <summary>
    /// Stores given amount of the given collectible to bag and updates its current weight. Returns amount of items left over.
    /// </summary>
    /// <param name="itemContainer">Collectible to be stored</param>
    /// <param name="amount">Amount of units to be stored</param>
    /// <returns>Amount of items left over</returns>
    public int Store(ItemContainer itemContainer, int amount)
    {
        int itemsLeft;        

        float addedWeight = itemContainer.ItemData.Weight * amount;

        if (currentWeight + addedWeight <= bagData.MaxWeight)
            itemsLeft = 0;
        else
        {
            float weightLeft = (currentWeight + addedWeight) - bagData.MaxWeight;
            itemsLeft = (int)(weightLeft / itemContainer.ItemData.Weight);
        }

        if (!storedItems.ContainsKey(itemContainer))
            storedItems.Add(itemContainer, amount - itemsLeft);
        else
            storedItems[itemContainer] += amount - itemsLeft;

        ReparentItemContainer(itemContainer);
        currentWeight += itemContainer.ItemData.Weight * (amount - itemsLeft);

        return itemsLeft;
    }

    private void ReparentItemContainer(ItemContainer value)
    {
        value.gameObject.transform.parent = transform;
    }

    /// <summary>
    /// Retrieves a single unit of the given collectible and updates current weight.
    /// </summary>
    /// <param name="itemContainer">Collectible to be retrieved</param>
    /// <returns>True if the object was retrieved</returns>
    public bool Retrieve(ItemContainer itemContainer)
    {
        bool contains;

        contains = storedItems.ContainsKey(itemContainer) ? true : false;
        currentWeight -= contains ? itemContainer.ItemData.Weight : 0;

        return contains;
    }

    /// <summary>
    /// Retrieves given amount of the given collectible and updates current weight.
    /// </summary>
    /// <param name="itemContainer">Collectible to be retrieved</param>
    /// <param name="amount">Amount of collectibles to be retrieved</param>
    /// <returns>Amount of collectibles retrieved</returns>
    public int Retrieve(ItemContainer itemContainer, int amount)
    {
        int amountToRetrieve;

        if (storedItems.ContainsKey(itemContainer))
        {
            amountToRetrieve = storedItems[itemContainer] >= amount ? amount : storedItems[itemContainer];
        }
        else
        {
            amountToRetrieve = 0;
        }

        if (storedItems[itemContainer] == 0)
            storedItems.Remove(itemContainer);

        currentWeight -= itemContainer.ItemData.Weight * amountToRetrieve;
        return amountToRetrieve;
    }

    public void Discard(ItemContainer itemData, int amount)
    {
        int amountToDiscard = Retrieve(itemData, amount);

        Collectible.CreateCollectible(itemData, SceneManager.Instance.CollectiblePrefab, transform.position + transform.forward);
    }

    void Start()
    {
        if (storedItems == null)
            storedItems = new Dictionary<ItemContainer, int>();
    }
}
