using UnityEngine;
using System.Collections;

public delegate void HealthEnded();

public class Health : MonoBehaviour
{
    public event HealthEnded OnHealthEnd;

    [SerializeField]
    private float currentHP;

    public float CurrentHP
    {
        get { return currentHP; }
    }

    [SerializeField]
    private float maxHP;

    public float MaxHP
    {
        get { return maxHP; }
        private set { maxHP = value; }
    }

    [SerializeField]
    private float totalDefense;

    public float TotalDefense
    {
        get { return totalDefense; }
        set { totalDefense = value; }
    }
    public void Damage(Ammo ammo)
    {
        float damageValue = ammo.damage;
        float damageReduction = totalDefense - ammo.penetration;

        damageValue -= damageReduction;

        Damage(damageValue);
    }

    private void Damage(float value)
    {
        currentHP = Mathf.Max(0, currentHP - value);
    }    
}
