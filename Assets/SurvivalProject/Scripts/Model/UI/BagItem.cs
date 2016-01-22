using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BagItem : MonoBehaviour
{
    public Text itemName;
    public ItemContainer item;

    public void Initialize(ItemContainer itemContainer)
    {
        item = itemContainer;
        itemName.text = itemContainer.ItemData.ItemName;
    }

    public void DisplayInfo()
    {
        UIController.Instance.CharacterMenu.InfoWindow.DisplayInfo(item);
    }

    public static void Create(GameObject bagItemPrefab, Transform parent, ItemContainer itemContainer)
    {
        GameObject bagItem = Instantiate(bagItemPrefab);
        bagItem.transform.SetParent(parent, false);
        bagItem.GetComponent<BagItem>().Initialize(itemContainer);
    }
}
