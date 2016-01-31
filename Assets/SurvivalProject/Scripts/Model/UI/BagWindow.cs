using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class BagWindow : UIWindow
{
    public GameObject bagItemPrefab;

    [SerializeField]
    private Transform gridTransform;

    [SerializeField]
    private List<BagItem> bagItemList;

    public void RefreshBag()
    {
        ClearBag();

        foreach (ItemData itemData in UIController.Instance.CharacterMenu.Character.EquippedBag.StoredItems)
        {
            bagItemList.Add(BagItem.CreateToggle(bagItemPrefab, gridTransform, itemData));            
        }
    }

    private void ClearBag()
    {
        foreach (BagItem item in bagItemList)
        {
            Destroy(item.gameObject);
        }

        bagItemList.Clear();
    }

    public override void Show()
    {
        base.Show();

        RefreshBag();
    }

    public override void Hide()
    {
        base.Hide();

        ClearBag();
    }

    public override void Start()
    {
        base.Start();

        bagItemList = new List<BagItem>();
    }
}
