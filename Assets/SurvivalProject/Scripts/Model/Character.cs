using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Firearm equippedFirearm;
    public Firearm EquippedFirearm
    {
        get
        {
            return equippedFirearm;
        }
        set
        {
            equippedFirearm = value;
        }
    }

    private CharacterInput characterInput;
    public CharacterInput CharacterInput { get { return characterInput; } }

    void Start()
    {
        characterInput = GetComponent<CharacterInput>();
    }
}
