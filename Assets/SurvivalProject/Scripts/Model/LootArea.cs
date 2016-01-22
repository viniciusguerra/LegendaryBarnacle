using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootArea : MonoBehaviour
{
    [SerializeField]
    private List<ICollectible> loot;



    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == Collectible.CollectibleTag)
        {

        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == Collectible.CollectibleTag)
        {

        }
    }
}
