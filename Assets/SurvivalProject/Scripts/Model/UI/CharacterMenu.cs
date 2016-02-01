using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterMenu : UIWindow
{
    [SerializeField]
    private Character character;
    public Character Character { get { return character; } }

    [SerializeField]
    private StatusWindow statusWindow;
    public StatusWindow StatusWindow { get { return statusWindow; } }

    [SerializeField]
    private EquipmentWindow equipmentWindow;
    public EquipmentWindow EquipmentWindow { get { return equipmentWindow; } }

    [SerializeField]
    private BagWindow bagWindow;
    public BagWindow BagWindow { get { return bagWindow; } }

    [SerializeField]
    private InfoWindow infoWindow;
    public InfoWindow InfoWindow { get { return infoWindow; } }

    [SerializeField]
    private ToggleGroup itemSelectionToggleGroup;
    public ToggleGroup ItemSelectionToggleGroup { get { return itemSelectionToggleGroup; } }

    public void SetDefaultSelection()
    {
        equipmentWindow.SetDefaultSelection();
    }

    public void SetDefaultSelection(bool value)
    {
        if(value)
            equipmentWindow.SetDefaultSelection();
    }

    public void HandleBagItem(BagItem bagItem)
    {
        if(!infoWindow.magazineEquipper.IsVisible)
        {
            SetDefaultSelection();
            bagItem.DisplayInfo();
        }
        else
        {
            infoWindow.magazineEquipper.HandleBagItem(bagItem);
        }        
    }

    public override void Show()
    {
        base.Show();

        statusWindow.Show();
        equipmentWindow.Show();
        bagWindow.Show();
        infoWindow.Show();
    }

    public override void Hide()
    {
        base.Hide();

        statusWindow.Hide();
        equipmentWindow.Hide();
        bagWindow.Hide();
        infoWindow.Hide();
    }

    public override void Toggle()
    {
        isVisible = !isVisible;

        if (isVisible)
            Show();
        else
            Hide();
    }
}
