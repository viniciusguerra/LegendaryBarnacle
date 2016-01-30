using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class BagItem : MonoBehaviour
{
    public Text itemNameText;
    public ItemData itemData;

    public void Initialize(ItemData itemData)
    {
        this.itemData = itemData;
        itemNameText.text = itemData.ItemName;
    }

    public void DisplayInfo()
    {
        UIController.Instance.CharacterMenu.InfoWindow.DisplayInfo(itemData);
    }

    public static BagItem CreateToggle(GameObject bagItemPrefab, Transform parent, ItemData itemData)
    {
        GameObject bagItemGameObject = Instantiate(bagItemPrefab);
        bagItemGameObject.transform.SetParent(parent, false);        
        bagItemGameObject.GetComponent<Toggle>().group = UIController.Instance.CharacterMenu.ItemSelectionToggleGroup;

        BagItem bagItem = bagItemGameObject.GetComponent<BagItem>();
        bagItem.Initialize(itemData);

        //adds listener for displaying info
        Toggle toggle = bagItem.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener((value) => UIController.Instance.CharacterMenu.HandleBagItem(bagItem));
        toggle.group = UIController.Instance.CharacterMenu.ItemSelectionToggleGroup;        

        return bagItem;        
    }

    public static BagItem CreateButton(GameObject bagItemPrefab, Transform parent, ItemData itemData)
    {
        GameObject bagItemGameObject = Instantiate(bagItemPrefab);
        bagItemGameObject.transform.SetParent(parent, false);

        BagItem bagItem = bagItemGameObject.GetComponent<BagItem>();
        bagItem.Initialize(itemData);

        //adds listener for displaying info
        Button button = bagItem.GetComponent<Button>();
        button.onClick.AddListener(() => UIController.Instance.CharacterMenu.HandleBagItem(bagItem));        

        return bagItem;
    }
}
