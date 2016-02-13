using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Health))]
public class Breakable : MonoBehaviour
{
    [SerializeField]
    private float shakeTime = 0.05f;

    [SerializeField]
    private Health health;
    public Health Health
    {
        get { return health; }
    }
    [SerializeField]
    private new Rigidbody rigidbody;    

    private void Shake(Damage damage)
    {
        Vector3 direction = transform.position - damage.Origin.position;
        rigidbody.AddForce(direction.normalized * damage.Value, ForceMode.Impulse);
    }

    private void TakeDamage(Damage damage)
    {
        StartCoroutine(ShakeCoroutine(damage));
    }

    private void Break(Damage damage)
    {
        rigidbody.isKinematic = false;

        Shake(damage);
    }

    private IEnumerator ShakeCoroutine(Damage damage)
    {
        rigidbody.isKinematic = false;

        Shake(damage);

        yield return new WaitForSeconds(shakeTime);

        rigidbody.velocity = Vector3.zero;

        yield return new WaitForEndOfFrame();

        rigidbody.isKinematic = true;
    }

    void Start()
    {
        if (health == null)
            health = GetComponent<Health>();

        health.OnTookDamage += TakeDamage;
        health.OnHealthEnded += Break;
    }
}
