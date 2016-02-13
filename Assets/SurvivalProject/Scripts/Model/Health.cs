using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Health : MonoBehaviour
{
    public event DamageReceived OnHealthEnded;
    public event DamageReceived OnTookDamage;

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
    public void ApplyDamage(AmmoData ammo, Transform origin)
    {
        Damage damage = new Damage(this, ammo, origin);

        ApplyDamage(damage);        
    }

    private void ApplyDamage(Damage damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage.Value);

        if (currentHP == 0)
            OnHealthEnded(damage);
        else
            OnTookDamage(damage);
    }    
}