using UnityEngine;
using System.Collections.Generic;
using System;

[SerializeField]
public class Bag : Equipment
{
    [SerializeField]
    private float maxWeight;
    public float MaxWeight { get { return maxWeight; } }

    [SerializeField]
    private float currentWeight;
    public float CurrentWeight { get { return currentWeight; } }

    private Dictionary<ICollectible, int> storedItems;

    #region Collectible
    [SerializeField]
    private string collectibleName;
    public override string Name
    {
        get
        {
            return collectibleName;
        }
    }

    [SerializeField]
    private float collectibleWeight;
    public override float Weight
    {
        get
        {
            return collectibleWeight;
        }
    }
    #endregion

    /// <summary>
    /// Stores a single unit of the given collectible to bag and updates its current weight. Returns if it was stored or not.
    /// </summary>
    /// <param name="collectible">Collectible to be stored</param>
    /// <returns>True if it was stored</returns>
    public bool Store(ICollectible collectible)
    {
        if(maxWeight <= currentWeight + collectible.Weight)
        {
            storedItems[collectible]++;
            currentWeight += collectible.Weight;
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
    /// <param name="collectible">Collectible to be stored</param>
    /// <param name="amount">Amount of units to be stored</param>
    /// <returns>Amount of items left over</returns>
    public int Store(ICollectible collectible, int amount)
    {
        int itemsLeft;        

        float addedWeight = collectible.Weight * amount;

        if (currentWeight + addedWeight <= maxWeight)
            itemsLeft = 0;
        else
        {
            float weightLeft = (currentWeight + addedWeight) - maxWeight;
            itemsLeft = (int)(weightLeft / collectible.Weight);
        }

        storedItems[collectible] += amount - itemsLeft;
        currentWeight += collectible.Weight * (amount - itemsLeft);

        return itemsLeft;
    }

    /// <summary>
    /// Retrieves a single unit of the given collectible and updates current weight.
    /// </summary>
    /// <param name="collectible">Collectible to be retrieved</param>
    /// <returns>True if the object was retrieved</returns>
    public bool Retrieve(ICollectible collectible)
    {
        bool contains;

        contains = storedItems.ContainsKey(collectible) ? true : false;
        currentWeight -= contains ? collectible.Weight : 0;

        return contains;
    }

    /// <summary>
    /// Retrieves given amount of the given collectible and updates current weight.
    /// </summary>
    /// <param name="collectible">Collectible to be retrieved</param>
    /// <param name="amount">Amount of collectibles to be retrieved</param>
    /// <returns>Amount of collectibles retrieved</returns>
    public int Retrieve(ICollectible collectible, int amount)
    {
        int amountToRetrieve;

        if (storedItems.ContainsKey(collectible))
        {
            amountToRetrieve = storedItems[collectible] >= amount ? amount : storedItems[collectible];
        }
        else
        {
            amountToRetrieve = 0;
        }

        currentWeight -= collectible.Weight * amountToRetrieve;
        return amountToRetrieve;
    }

    public void Discard(ICollectible collectible, int amount)
    {
        int amountToDiscard = Retrieve(collectible, amount);

        Collectible.CreateCollectible(collectible, amountToDiscard, transform.position + transform.forward);
    }

    void Start()
    {
        if (storedItems == null)
            storedItems = new Dictionary<ICollectible, int>();
    }
}
