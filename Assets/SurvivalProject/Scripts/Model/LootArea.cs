using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootArea : MonoBehaviour
{
    [SerializeField]
    private Character character;
    [SerializeField]
    private List<Collectible> collectibleList;
    [SerializeField]
    private Collectible selectedCollectible;
    public Collectible SelectedCollectible
    {
        get
        {
            return selectedCollectible;
        }
        private set
        {
            selectedCollectible = value;
        }
    }
    

    public void CycleItems()
    {
        int nextIndex = 0;

        if (selectedCollectible != null)
        {
            nextIndex = collectibleList.FindIndex(x => x == selectedCollectible) + 1;

            if (nextIndex == collectibleList.Count)
                nextIndex = 0;
        }

        selectedCollectible = collectibleList[nextIndex];
    }

    public void StoreSelectedItem()
    {
        if (character.EquippedBag.Store(selectedCollectible.Item) == true)
        {
            Destroy(selectedCollectible);
            selectedCollectible = null;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == Collectible.CollectibleTag)
        {
            Collectible collectible = collider.GetComponent<Collectible>();

            collectibleList.Add(collectible);

            if (selectedCollectible == null)
                SelectedCollectible = collectible;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == Collectible.CollectibleTag)
        {
            Collectible collectible = collider.GetComponent<Collectible>();

            collectibleList.Remove(collider.GetComponent<Collectible>());

            if (selectedCollectible == collectible)
                SelectedCollectible = null;
        }
    }

    void Start()
    {
        collectibleList = new List<Collectible>();
    }
}
