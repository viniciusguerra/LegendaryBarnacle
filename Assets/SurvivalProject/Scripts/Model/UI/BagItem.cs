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

    public void DisplayInfo(bool value)
    {
        if(value)
        {
            DisplayInfo();
        }
    }

    public static BagItem Create(GameObject bagItemPrefab, Transform parent, ItemContainer itemContainer)
    {
        GameObject bagItemGameObject = Instantiate(bagItemPrefab);
        bagItemGameObject.transform.SetParent(parent, false);

        BagItem bagItem = bagItemGameObject.GetComponent<BagItem>();
        bagItem.Initialize(itemContainer);

        //adds listener for displaying info
        var button = bagItem.GetComponent<Button>();

        if (button != null)
        {
            bagItem.GetComponent<Button>().onClick.AddListener(() => bagItem.DisplayInfo());
        }
        else
        {
            var toggle = bagItem.GetComponent<Toggle>();

            toggle.onValueChanged.AddListener((value) => bagItem.DisplayInfo(value));
            toggle.group = UIController.Instance.CharacterMenu.ItemSelectionToggleGroup;
        }

        return bagItem;        
    }
}
