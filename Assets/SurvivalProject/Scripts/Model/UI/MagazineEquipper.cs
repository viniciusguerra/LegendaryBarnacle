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
    private List<MagazineUI> magazineUiList;
    private MagazineUI selectedMagazine;

    public GameObject magazineUiPrefab;

    public int LoadAmmoIntoSelectedMagazine(AmmoData ammoData, int amount)
    {
        return selectedMagazine.LoadMagazine(ammoData, amount);
    }

    public void EquipSelectedMagazine(MagazineData magazineData)
    {
        //stores current magazine
        if (selectedMagazine.MagazineData != null)
        {
            UIController.Instance.CharacterMenu.Character.EquippedBag.Store(selectedMagazine.MagazineData);
            vestData.StoredMagazines.RemoveAt(vestData.StoredMagazines.FindIndex(x => x == selectedMagazine.MagazineData));
        }

        MagazineData magazineToAdd = UIController.Instance.CharacterMenu.Character.EquippedBag.Retrieve(magazineData) ? magazineData : null;

        vestData.StoredMagazines.Add(magazineToAdd);

        UIController.Instance.CharacterMenu.BagWindow.RefreshBag();
        UIController.Instance.CharacterMenu.SetDefaultSelection();

        ClearMagazineToggles();
        CreateMagazineToggles();
    }

    public void HandleBagItem(BagItem bagItem)
    {
        if (bagItem.itemData.GetType().Equals(typeof(MagazineData)))
        {
            EquipSelectedMagazine(bagItem.itemData as MagazineData);
            return;
        }

        StackData stack = bagItem.itemData as StackData;

        if (stack != null && selectedMagazine != null && selectedMagazine.MagazineData != null)
        {
            if (stack.ContainedItemData.GetType().Equals(typeof(AmmoData)))
            {
                stack.Amount = LoadAmmoIntoSelectedMagazine(stack.ContainedItemData as AmmoData, stack.Amount);
                ClearMagazineToggles();
                CreateMagazineToggles();
            }
        }
    }

    private void SelectMagazine(bool value, MagazineUI magazine)
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

            MagazineUI magazineUi = magazineUiGameObject.GetComponent<MagazineUI>();
            magazineUi.Initialize(magazineData);

            Toggle toggle = magazineUiGameObject.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(((value) => SelectMagazine(value, magazineUi)));
            toggle.group = magazineToggleGroup;

            magazineUiList.Add(magazineUi);
        }
    }

    private void ClearMagazineToggles()
    {
        foreach (MagazineUI magazineUi in magazineUiList)
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
        magazineUiList = new List<MagazineUI>();
    }
}
