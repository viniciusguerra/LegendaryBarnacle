using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BagItem : MonoBehaviour
{
    public Text itemName;
    public ICollectible collectible;

    public void Initialize(ICollectible collectible)
    {
        this.collectible = collectible;
        itemName.text = collectible.Name;
    }

    public static void Create(GameObject bagItemPrefab, Transform parent, ICollectible collectible)
    {
        GameObject bagItem = Instantiate(bagItemPrefab);
        bagItem.transform.SetParent(parent, false);
        bagItem.GetComponent<BagItem>().Initialize(collectible);
    }
}
