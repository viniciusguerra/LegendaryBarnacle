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
            if (collectibleList.Exists(x => x == selectedCollectible))
                nextIndex = collectibleList.FindIndex(x => x == selectedCollectible) + 1;
            else
                selectedCollectible = null;

            if (nextIndex == collectibleList.Count)
                nextIndex = 0;
        }

        selectedCollectible = collectibleList.Count > 0 ? collectibleList[nextIndex] : null;
    }

    public void StoreSelectedItem()
    {
        ItemData itemToStore = null;

        if (selectedCollectible != null)
            itemToStore = selectedCollectible.ItemData;
        else
            return;

        bool storeSuccessful = character.EquippedBag.Store(itemToStore);

        if (storeSuccessful == true)
        {
            selectedCollectible.gameObject.SetActive(false);
            collectibleList.Remove(selectedCollectible);
            selectedCollectible = null;

            CycleItems();
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
