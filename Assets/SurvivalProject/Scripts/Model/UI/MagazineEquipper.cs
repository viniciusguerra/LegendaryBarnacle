using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MagazineEquipper : UIWindow
{
    private VestData vestData;

    [SerializeField]
    private ToggleGroup magazineToggleGroup;
    [SerializeField]
    private Transform magazineGridViewTransform;
    private List<EquippableMagazineUI> magazineUiList;
    private EquippableMagazineUI selectedMagazine;

    public GameObject magazineUiPrefab;

    private StackData[] LoadAmmoIntoSelectedMagazine(AmmoData ammoData, int amount)
    {
        return selectedMagazine.LoadMagazine(ammoData, amount);
    }

    public void EquipSelectedMagazine(MagazineData magazineData)
    {
        if (selectedMagazine.MagazineData != null)        
            UIController.Instance.CharacterMenu.Character.EquippedBag.Store(vestData.RetrieveMagazine(selectedMagazine.MagazineData), 1);
        
        if (vestData.StoreMagazine(magazineData) != null)
            UIController.Instance.CharacterMenu.Character.EquippedBag.Store(magazineData, 1);

        UIController.Instance.CharacterMenu.BagWindow.RefreshBag();
        UIController.Instance.CharacterMenu.SetDefaultSelection();

        ClearMagazineToggles();
        CreateMagazineToggles();
    }

    public void HandleBagItem(BagItem bagItem)
    {
        if (bagItem.itemData.GetType().Equals(typeof(MagazineData)))
        {
            MagazineData magazineToAdd = bagItem.itemData as MagazineData;

            StackData stackData = UIController.Instance.CharacterMenu.Character.EquippedBag.Retrieve(magazineToAdd, 1);
            magazineToAdd = stackData != null ? stackData.ContainedItem as MagazineData : null;

            if(magazineToAdd != null)
                EquipSelectedMagazine(magazineToAdd);

            return;
        }

        StackData stack = bagItem.itemData as StackData;

        if (stack != null && selectedMagazine != null && selectedMagazine.MagazineData != null)
        {
            if (stack.ContainedItem.GetType().Equals(typeof(AmmoData)))
            {
                //returns old ammo from magazine
                stack = UIController.Instance.CharacterMenu.Character.EquippedBag.Retrieve(stack.ContainedItem, stack.Amount);
                StackData[] leftOverAmmoStacks = LoadAmmoIntoSelectedMagazine(stack.ContainedItem as AmmoData, stack.Amount);

                ClearMagazineToggles();
                CreateMagazineToggles();

                //if there is any left over ammo, store it
                if (leftOverAmmoStacks != null)
                {
                    foreach (StackData leftOverStack in leftOverAmmoStacks)
                    {
                        if(leftOverStack != null && !string.IsNullOrEmpty(leftOverStack.ItemName))
                            UIController.Instance.CharacterMenu.Character.EquippedBag.Store(leftOverStack, 1);
                    }
                    
                    UIController.Instance.CharacterMenu.BagWindow.RefreshBag();
                }
            }
        }
    }

    private void SelectMagazine(bool value, EquippableMagazineUI magazine)
    {
        if (value)
            selectedMagazine = magazine;
    }

    public void Initialize(VestData vestData)
    {
        this.vestData = vestData;

        Show();
        CreateMagazineToggles();       
    }

    private void CreateMagazineToggles()
    {
        for (int i = 0; i < vestData.MagazineCapacity; i++)
        {
            GameObject magazineUiGameObject = Instantiate(magazineUiPrefab);
            magazineUiGameObject.transform.SetParent(magazineGridViewTransform, false);

            MagazineData magazineData;

            if (vestData.StoredMagazines.Count > 0 && i < vestData.StoredMagazines.Count)
            {
                magazineData = vestData.StoredMagazines[i] as MagazineData;
            }
            else
            {
                magazineData = null;
            }

            EquippableMagazineUI magazineUi = magazineUiGameObject.GetComponent<EquippableMagazineUI>();
            magazineUi.Initialize(magazineData);

            Toggle toggle = magazineUiGameObject.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(((value) => SelectMagazine(value, magazineUi)));
            toggle.group = magazineToggleGroup;

            magazineUiList.Add(magazineUi);
        }
    }

    private void ClearMagazineToggles()
    {
        foreach (EquippableMagazineUI magazineUi in magazineUiList)
        {
            Destroy(magazineUi.gameObject);
        }

        magazineUiList.Clear();
    }

    public override void Hide()
    {
        base.Hide();

        vestData = null;
        ClearMagazineToggles();
    }

    void Awake()
    {
        magazineUiList = new List<EquippableMagazineUI>();
    }
}
