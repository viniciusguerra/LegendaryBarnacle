using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHP;

    public int CurrentHP
    {
        get { return currentHP; }
    }

    [SerializeField]
    private int maxHP;

    public int MaxHP
    {
        get { return maxHP; }
        private set { maxHP = value; }
    }

    public void Damage(int value)
    {
        currentHP = Mathf.Max(0, currentHP - value);
    }

    public void Damage(Ammo ammo)
    {

    }
}
