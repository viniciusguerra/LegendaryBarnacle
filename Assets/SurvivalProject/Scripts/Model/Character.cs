﻿using UnityEngine;
using System;
using System.Collections;

public class Character : MonoBehaviour
{
    #region Equipment
    [SerializeField]
    private Firearm wieldedFirearm;
    public Firearm WieldedFirearm
    {
        get { return wieldedFirearm; }
    }

    [SerializeField]
    private Holster equippedHolster;
    public Holster EquippedHolster
    {
        get { return equippedHolster; }
    }

    [SerializeField]
    private Vest equippedVest;
    public Vest EquippedVest
    {
        get { return equippedVest; }
    }

    [SerializeField]
    private Clothing equippedClothing;
    public Clothing EquippedClothing
    {
        get { return equippedClothing; }
    }

    [SerializeField]
    private Bag equippedBag;
    public Bag EquippedBag
    {
        get { return equippedBag; }
    }
    #endregion

    [Space(20)]

    #region Health
    [SerializeField]
    private Health health;
    public Health Health
    {
        get { return health; }
    }
    #endregion

    //[SerializeField]
    //private CharacterInput characterInput;
    //public CharacterInput CharacterInput { get { return characterInput; } }

    public void Equip(Equipment equipment)
    {
        Equipment equipmentToStore = null;

        Type equipmentType = equipment.GetType();

        switch (equipmentType.ToString())
        {
            case "Bag":
            {
                equipmentToStore = equippedBag;
                equippedBag = equipment as Bag;

                break;
            }
            case "Holster":
            {
                equipmentToStore = equippedHolster;
                equippedHolster = equipment as Holster;

                break;
            }
            case "Vest":
            {
                equipmentToStore = equippedVest;
                equippedVest = equipment as Vest;

                UpdateTotalDefense();

                break;
            }
            case "Clothing":
            {
                equipmentToStore = equippedClothing;
                equippedClothing = equipment as Clothing;

                UpdateTotalDefense();

                break;
            }
            case "Firearm":
            {
                //if no holster is equipped, store firearm
                if (equippedHolster != null)
                {
                    equipmentToStore = equippedHolster.Retrieve();
                    equippedHolster.Store(equipment as Firearm);
                }
                else
                {
                    equipmentToStore = equipment;
                }

                break;
            }                        
            default:
                break;
        }

        //stores any previously equipped items and discards if it doesn't fit in the bag
        if (equipmentToStore != null)
        {
            equippedBag.Store(equipmentToStore.ItemData, 1);
        }
    }

    private void UpdateTotalDefense()
    {
        float totalDefense = 0;

        totalDefense += ((ClothingData)equippedClothing.ItemData).Defense;
        totalDefense += ((ClothingData)equippedClothing.ItemData).Defense;

        Health.TotalDefense = totalDefense;

        return;
    }

    public void DrawFromHolster()
    {
        float timeToWield;
        Firearm firearmToWield = equippedHolster.Draw(out timeToWield);

        StartCoroutine(DrawCoroutine(firearmToWield, timeToWield));
    }

    private IEnumerator DrawCoroutine(Firearm firearmToWield, float timeToWield)
    {
        yield return new WaitForSeconds(timeToWield);
        wieldedFirearm = firearmToWield;
    }

    public void HolsterFirearm()
    {
        StopCoroutine("DrawCoroutine");
        wieldedFirearm = null;
    }

    void Start()
    {
        UpdateTotalDefense();
    }
}
