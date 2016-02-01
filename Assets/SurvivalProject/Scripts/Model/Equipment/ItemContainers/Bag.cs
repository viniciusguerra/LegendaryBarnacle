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
        protected set { bagData = value as BagData; }
    }

    public float MaxWeight { get { return bagData.MaxWeight; } }

    public float CurrentWeight { get { return bagData.CurrentWeight; } private set { bagData.CurrentWeight = value; } }

    public List<ItemData> StoredItems
    {
        get { return bagData.StoredItems; }
        private set { bagData.StoredItems = value; }
    }

    /// <summary>
    /// Stores given amount of the given collectible to bag and updates its current weight. Returns amount of items left over.
    /// </summary>
    /// <param name="itemData">Collectible to be stored</param>
    /// <param name="amount">Amount of units to be stored</param>
    public void Store(ItemData itemData, int amount)
    {
        int itemsLeft = 0;

        float addedWeight = itemData.Weight * amount;

        if(CurrentWeight + addedWeight > MaxWeight)
        {
            itemsLeft = CalculateLeftOver(itemData.Weight, addedWeight);
        }

        int amountToStore = amount - itemsLeft;

        //if item is stack, add to a current stack
        if (itemData.GetType().Equals(typeof(StackData)))
        {
            StackData stack = itemData as StackData;
            AddStack(stack.ContainedItem, stack.Amount);
        }
        else
        {
            //add stack if there is more than one item to store
            if (amountToStore > 1)
            {
                AddStack(itemData, amountToStore);
            }
            else
            {
                //add single unit
                if (amountToStore == 1)
                    StoredItems.Add(itemData);
            }
        }

        //add weight
        CurrentWeight += itemData.Weight * (amountToStore);

        //discard leftovers
        if (itemsLeft > 0)
            Discard(itemData, itemsLeft);
    }

    private int CalculateLeftOver(float unitWeight, float totalWeight)
    {
        if (CurrentWeight + totalWeight <= MaxWeight)
        {
            return 0;
        }
        else
        {
            return CalculateLeftOver(unitWeight, totalWeight - unitWeight) + 1;
        }
    }

    /// <summary>
    /// Retrieves given amount of the given collectible and updates current weight.
    /// </summary>
    /// <param name="itemData">Collectible to be retrieved</param>
    /// <param name="amount">Amount of collectibles to be retrieved</param>
    /// <returns>Stack of that item or null</returns>
    public StackData Retrieve(ItemData itemData, int amount)
    {
        StackData stack = null;

        //if a stack exists, return the possible amount
        stack = RemoveStack(itemData, amount);
        
        if(stack == null)
        {
            //if a single unit exists, return a stack with it
            if (StoredItems.Contains(itemData))
            {
                StoredItems.Remove(itemData);
                stack = new StackData(itemData, 1);
            }
        }

        return stack;
    }

    public void Discard(ItemData itemData, int amount)
    {
        Collectible.CreateCollectible(itemData, amount, SceneManager.Instance.CollectiblePrefab, transform.position + transform.forward);
    }

    private void AddStack(ItemData itemData, int amount)
    {
        //if a unit of the item exists, it is removed and a stack is added instead
        if (StoredItems.Contains(itemData))
        {
            StoredItems.Remove(itemData);
            amount++;
        }

        StackData stack;

        //if a stack of the item is found, it is removed and the current amount is added
        foreach (ItemData item in StoredItems)
        {
            stack = item as StackData;

            if (stack != null && stack.ContainedItem.Equals(itemData))
            {                
                StoredItems.Remove(item);
                amount += stack.Amount;
                break;
            }
        }

        stack = new StackData(itemData, amount);
        StoredItems.Add(stack);
    }

    private StackData RemoveStack(ItemData itemData, int amount)
    {
        if(amount == 1)
        {
            if (StoredItems.Contains(itemData))
            {
                StoredItems.Remove(itemData);
                return new StackData(itemData, 1);
            }            
        }

        StackData currentStack = null;

        foreach (ItemData item in StoredItems)
        {
            currentStack = item as StackData;

            if (currentStack != null && currentStack.ContainedItem.Equals(itemData))
            {
                //remove stack containing the item from the bag
                StoredItems.Remove(item);

                int leftOverAmount = currentStack.Amount - amount;

                if (leftOverAmount < 0)
                {               
                    //return the stack amount if it was less than the given     
                    currentStack = new StackData(itemData, amount + leftOverAmount);
                    break;
                }
                else
                {
                    if(leftOverAmount == 0)
                    {
                        //return the given amount if none was left
                        currentStack = new StackData(itemData, amount);
                        break;
                    }
                    else
                    {
                        //return the given amount if at least one was left
                        currentStack = new StackData(itemData, amount);

                        //store back a leftover stack
                        StoredItems.Add(new StackData(itemData, currentStack.Amount - amount) as ItemData);
                        break;
                    }
                }
            }
        }

        return currentStack;
    }

    private bool StackExists(ItemData itemData)
    {
        StackData stack;

        foreach (ItemData item in StoredItems)
        {
            stack = item as StackData;

            if (stack != null && stack.ContainedItem == itemData)
            {
                return true; 
            }
        }

        return false;
    }

    void Start()
    {
        if (StoredItems == null)
            StoredItems = new List<ItemData>();
    }
}
