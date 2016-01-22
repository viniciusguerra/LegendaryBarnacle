﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BagWindow : UIWindow
{
    public GameObject bagItemPrefab;

    [SerializeField]
    private Transform gridTransform;

    private List<BagItem> bagItemList;

    public override void Show()
    {
        base.Show();

        foreach (KeyValuePair<ICollectible, int> pair in UIController.Instance.CharacterMenu.Character.EquippedBag.StoredItems)
        {
            BagItem.Create(bagItemPrefab, gridTransform, pair.Key);
        }
    }

    public override void Hide()
    {
        base.Hide();

        foreach(BagItem item in bagItemList)
        {
            Destroy(item.gameObject);
        }

        bagItemList.Clear();
    }

    public override void Start()
    {
        base.Start();

        bagItemList = new List<BagItem>();
    }
}
