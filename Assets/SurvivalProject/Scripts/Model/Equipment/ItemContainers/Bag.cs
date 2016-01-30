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

    public float CurrentWeight { get { return bagData.CurrentWeight; } private set { bagData.CurrentWeight = value; } }

    public Dictionary<ItemData, int> StoredItems
    {
        get { return bagData.StoredItems; }
        private set { bagData.StoredItems = value; }
    }

    /// <summary>
    /// Stores a single unit of the given collectible to bag and updates its current weight. Returns if it was stored or not.
    /// </summary>
    /// <param name="itemData">Item to be stored</param>
    /// <returns>True if it was stored</returns>
    public bool Store(ItemData itemData)
    {
        if(bagData.MaxWeight >= CurrentWeight + itemData.Weight)
        {
            if (!StoredItems.ContainsKey(itemData))            
                StoredItems.Add(itemData, 1);
            else
                StoredItems[itemData]++;

            CurrentWeight += itemData.Weight;
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
    /// <param name="itemData">Collectible to be stored</param>
    /// <param name="amount">Amount of units to be stored</param>
    /// <returns>Amount of items left over</returns>
    public int Store(ItemData itemData, int amount)
    {
        int itemsLeft;        

        float addedWeight = itemData.Weight * amount;

        if (CurrentWeight + addedWeight <= bagData.MaxWeight)
            itemsLeft = 0;
        else
        {
            float weightLeft = (CurrentWeight + addedWeight) - bagData.MaxWeight;
            itemsLeft = (int)(weightLeft / itemData.Weight);
        }

        if (!StoredItems.ContainsKey(itemData))
            StoredItems.Add(itemData, amount - itemsLeft);
        else
            StoredItems[itemData] += amount - itemsLeft;

        CurrentWeight += itemData.Weight * (amount - itemsLeft);

        return itemsLeft;
    }

    /// <summary>
    /// Retrieves a single unit of the given collectible and updates current weight.
    /// </summary>
    /// <param name="itemData">Collectible to be retrieved</param>
    /// <returns>True if the object was retrieved</returns>
    public bool Retrieve(ItemData itemData)
    {
        bool contains;

        if (StoredItems.ContainsKey(itemData) && StoredItems[itemData] > 0)
        {
            contains = true;
            CurrentWeight -= itemData.Weight;
            StoredItems[itemData] -= 1;

            if (StoredItems[itemData] == 0)
                StoredItems.Remove(itemData);
        }
        else
            contains = false;        

        return contains;
    }

    /// <summary>
    /// Retrieves given amount of the given collectible and updates current weight.
    /// </summary>
    /// <param name="itemData">Collectible to be retrieved</param>
    /// <param name="amount">Amount of collectibles to be retrieved</param>
    /// <returns>Amount of collectibles retrieved</returns>
    public int Retrieve(ItemData itemData, int amount)
    {
        int amountToRetrieve;

        if (StoredItems.ContainsKey(itemData))
        {
            amountToRetrieve = StoredItems[itemData] >= amount ? amount : StoredItems[itemData];
        }
        else
        {
            amountToRetrieve = 0;
        }

        if (StoredItems[itemData] == 0)
            StoredItems.Remove(itemData);

        CurrentWeight -= itemData.Weight * amountToRetrieve;
        return amountToRetrieve;
    }

    public void Discard(ItemData itemData, int amount)
    {
        int amountToDiscard = Retrieve(itemData, amount);

        Collectible.CreateCollectible(itemData, SceneManager.Instance.CollectiblePrefab, transform.position + transform.forward);
    }

    void Start()
    {
        if (StoredItems == null)
            StoredItems = new Dictionary<ItemData, int>();
    }
}
