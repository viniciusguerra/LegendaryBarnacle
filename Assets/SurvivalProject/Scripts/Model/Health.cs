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
    private float defence;

    public float Defence
    {
        get { return defence; }
        set { defence = value; }
    }
    public void Damage(Ammo ammo)
    {
        float damageValue;
        float damageReduction = defence;
        

    }

    private void Damage(float value)
    {
        currentHP = Mathf.Max(0, currentHP - value);
    }    
}
