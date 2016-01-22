using UnityEngine;
using System.Collections;

public class UIController : Singleton<UIController>
{
    [SerializeField]
    private HUD hud;
    public HUD HUD
    {
        get { return hud; }
    }

    [SerializeField]
    private CharacterMenu characterMenu;

    public CharacterMenu CharacterMenu
    {
        get { return characterMenu; }
    }
}
