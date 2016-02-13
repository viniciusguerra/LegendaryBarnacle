using UnityEngine;
using System.Collections;

public interface IDamageSource
{
    float Damage { get; }
    float Stagger { get; }
}

public delegate void DamageReceived(Damage attack);

public class Damage
{
    protected Health target;
    public Health Target
    {
        get { return target; }
    }

    protected IDamageSource source;
    public IDamageSource Source
    {
        get { return source; }
    }

    protected float value;
    public float Value
    {
        get { return value; }
    }

    protected Transform origin;
    public Transform Origin
    {
        get { return origin; }
    }

    protected virtual void CalculateDamage()
    {
        value = source.Damage - target.TotalDefense;
    }

    public Damage(Health target, IDamageSource source, Transform origin)
    {
        this.target = target;
        this.source = source;
        this.origin = origin;

        CalculateDamage();
    }
}
