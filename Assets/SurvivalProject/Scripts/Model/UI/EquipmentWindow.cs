using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EquipmentWindow : UIWindow
{
    [SerializeField]
    private ToggleGroup toggleGroup;

    [SerializeField]
    private Toggle firearmToggle;
    [SerializeField]
    private Toggle bagToggle;
    [SerializeField]
    private Toggle holsterToggle;
    [SerializeField]
    private Toggle magazineToggle;
    [SerializeField]
    private Toggle vestToggle;
    [SerializeField]
    private Toggle clothingToggle;

    //For test, is being called by toggles when they are clicked
    public void DisplayInformation(int equip)
    {
        if (toggleGroup.AnyTogglesOn())
        {
            switch (equip)
            {
                case 0:
                {
                    UIController.Instance.CharacterMenu.InfoWindow.DisplayInfo(UIController.Instance.CharacterMenu.Character.EquippedHolster.EquippedFirearm.ItemData);
                    break;
                }
                case 1:
                {
                    UIController.Instance.CharacterMenu.InfoWindow.DisplayInfo(UIController.Instance.CharacterMenu.Character.EquippedBag.ItemData);
                    break;
                }
                case 2:
                {
                    UIController.Instance.CharacterMenu.InfoWindow.DisplayInfo(UIController.Instance.CharacterMenu.Character.EquippedHolster.ItemData);
                    break;
                }
                case 3:
                {
                    UIController.Instance.CharacterMenu.InfoWindow.DisplayInfo(UIController.Instance.CharacterMenu.Character.EquippedHolster.EquippedFirearm.CurrentMagazine);
                    break;
                }
                case 4:
                {
                    UIController.Instance.CharacterMenu.InfoWindow.DisplayInfo(UIController.Instance.CharacterMenu.Character.EquippedVest.ItemData);
                    break;
                }
                case 5:
                {
                    UIController.Instance.CharacterMenu.InfoWindow.DisplayInfo(UIController.Instance.CharacterMenu.Character.EquippedClothing.ItemData);
                    break;
                }
                default:
                break;
            }
        }
    }
    
    public void SetDefaultSelection()
    {
        firearmToggle.Select();
    }

    public override void Show()
    {
        base.Show();

        SetDefaultSelection();
    }

    public override void Hide()
    {
        base.Hide();

        toggleGroup.SetAllTogglesOff();
    }
}
