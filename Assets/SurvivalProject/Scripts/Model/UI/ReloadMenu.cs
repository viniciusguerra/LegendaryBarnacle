using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ReloadMenu : UIWindow
{
    [SerializeField]
    private Character player;

    [SerializeField]
    private Transform magazineGridTransform;

    private List<MagazineData> validMagazines;
    private List<ReloadableMagazineUI> reloadableMagazines;

    [SerializeField]
    private GameObject reloadableMagazinePrefab;
    [SerializeField]
    private Button storeMagazineButton;

    private void GetValidMagazines()
    {
        foreach (MagazineData magazineData in UIController.Instance.CharacterMenu.Character.EquippedVest.StoredMagazines)
        {
            if (UIController.Instance.CharacterMenu.Character.EquippedHolster.EquippedFirearm.Caliber == magazineData.Caliber)
                validMagazines.Add(magazineData);
        }
    }

    private void CreateReloadableMagazinesList()
    {
        foreach (MagazineData magazineData in validMagazines)
        {
            MagazineData currentMagazineData = magazineData;
            GameObject magazineGameObject = Instantiate(reloadableMagazinePrefab);
            magazineGameObject.transform.SetParent(magazineGridTransform, false);

            ReloadableMagazineUI magazineUi = magazineGameObject.GetComponent<ReloadableMagazineUI>();
            magazineUi.MagazineData = currentMagazineData;

            Button button = magazineGameObject.GetComponent<Button>();
            button.onClick.AddListener((() => RetrieveFromVestAndReload(currentMagazineData)));

            reloadableMagazines.Add(magazineUi);
        }
    }

    private void RetrieveFromVestAndReload(MagazineData magazineData)
    {
        MagazineData magazineToEquip = player.EquippedVest.VestData.RetrieveMagazine(magazineData);
        MagazineData magazineToStore = player.EquippedHolster.EquippedFirearm.LoadMagazine(magazineToEquip);

        if(magazineToStore != null)
            player.EquippedVest.VestData.StoreMagazine(magazineToStore);

        Hide();
    }

    public void StoreLoadedMagazine()
    {
        MagazineData magazineToStore = player.EquippedHolster.EquippedFirearm.LoadMagazine(null);

        if(magazineToStore != null && !string.IsNullOrEmpty(magazineToStore.ItemName))
            player.EquippedVest.VestData.StoreMagazine(magazineToStore);

        Hide();
    }

    private void ClearMagazineList()
    {
        validMagazines.Clear();

        foreach (ReloadableMagazineUI ui in reloadableMagazines)
        {
            Destroy(ui.gameObject);
        }

        reloadableMagazines.Clear();
    }

    public override void Show()
    {
        base.Show();

        GetValidMagazines();
        CreateReloadableMagazinesList();

        storeMagazineButton.Select();
    }

    public override void Hide()
    {
        base.Hide();

        ClearMagazineList();
    }

    public override void Toggle()
    {
        if (!isVisible)
            Show();
        else
            Hide();
    }

    public override void Start()
    {
        base.Start();

        validMagazines = new List<MagazineData>();
        reloadableMagazines = new List<ReloadableMagazineUI>();
    }
}
