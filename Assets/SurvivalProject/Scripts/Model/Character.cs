using UnityEngine;
using System;
using System.Collections;

public class Character : MonoBehaviour
{
    #region Health
    private Health health;

    public Health Health
    {
        get { return health; }
    }
    #endregion

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

    private CharacterInput characterInput;
    public CharacterInput CharacterInput { get { return characterInput; } }

    public void Equip(Equipment equipment)
    {
        Equipment leftOverEquipment = null;
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
            case "Clothing":
            {
                equipmentToStore = equippedClothing;
                equippedClothing = equipment as Clothing;

                Health.Defence = equippedClothing.Defence;

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

        //stores any previously equipped items
        if (equipmentToStore != null)
        {
            leftOverEquipment = equippedBag.Store(equipmentToStore) ? null : equipmentToStore;

            //discards previously equipped item if it can't be stored
            if (leftOverEquipment != null)
                Collectible.CreateCollectible(leftOverEquipment, 1, transform.position + transform.forward);
        }
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
        characterInput = GetComponent<CharacterInput>();
        health = GetComponent<Health>();
    }
}
