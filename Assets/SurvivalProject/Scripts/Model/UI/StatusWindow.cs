using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StatusWindow : UIWindow
{
    [SerializeField]
    public Text characterNameText;
    [SerializeField]
    public Text hpValue;
    [SerializeField]
    public Text stressValue;
    [SerializeField]
    public Text resilienceValue;
    [SerializeField]
    public Text enduranceValue;
    [SerializeField]
    public Text techniqueValue;
    [SerializeField]
    public Text totalDefenseValue;
    [SerializeField]
    public Text vestDefenseValue;
    [SerializeField]
    public Text clothingDefenseValue;
    [SerializeField]
    public Text drawSpeedValue;
    [SerializeField]
    public Text magazineCapacityValue;
    [SerializeField]
    public Text weightCapacityValue;

    private void UpdateValues()
    {
        characterNameText.text = UIController.Instance.CharacterMenu.Character.name;
        hpValue.text = UIController.Instance.CharacterMenu.Character.Health.CurrentHP.ToString() + '/' + UIController.Instance.CharacterMenu.Character.Health.MaxHP.ToString();
        //stressValue.text = UIController.Instance.CharacterMenu.Character.CurrentStress.ToString() + '/' + UIController.Instance.CharacterMenu.Character.MaxStress.ToString();
        //resilienceValue.text = UIController.Instance.CharacterMenu.Character.Resilience;
        //enduranceValue.text = UIController.Instance.CharacterMenu.Character.Endurance;
        //techniqueValue.text = UIController.Instance.CharacterMenu.Character.Technique;
        totalDefenseValue.text = UIController.Instance.CharacterMenu.Character.Health.TotalDefense.ToString();
        vestDefenseValue.text = UIController.Instance.CharacterMenu.Character.EquippedVest.Defense.ToString();
        clothingDefenseValue.text = UIController.Instance.CharacterMenu.Character.EquippedClothing.Defense.ToString();
        drawSpeedValue.text = UIController.Instance.CharacterMenu.Character.EquippedHolster.DrawTime.ToString() + 's';
        magazineCapacityValue.text = UIController.Instance.CharacterMenu.Character.EquippedVest.MagazineCount.ToString() + '/' + UIController.Instance.CharacterMenu.Character.EquippedVest.MagazineCapacity.ToString();
        weightCapacityValue.text = UIController.Instance.CharacterMenu.Character.EquippedBag.CurrentWeight.ToString() + '/' + UIController.Instance.CharacterMenu.Character.EquippedBag.MaxWeight.ToString() + "Kg";
    }

    void Update()
    {
        if(IsVisible)
        {
            UpdateValues();
        }
    }
}
