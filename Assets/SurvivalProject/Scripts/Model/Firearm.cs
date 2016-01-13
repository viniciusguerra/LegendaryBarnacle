using UnityEngine;
using System.Collections;

public class Firearm : MonoBehaviour
{
    public Character wielder;

    public Ammo currentAmmo;

    public Vector3 barrelTip;
    
    private Ray aimRay;
    private RaycastHit rayHitInfo;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private FirearmController controller;
    public FirearmController Controller
    {
        get
        {
            return controller;
        }
    }

    public void Shoot()
    {
        if (rayHitInfo.rigidbody != null)
        {
            rayHitInfo.rigidbody.AddForce(aimRay.direction.normalized * 100, ForceMode.Impulse);
        }
    }    

    void Start()
    {
        aimRay = new Ray();

        controller = animator.GetBehaviour<FirearmController>();
        controller.firearm = this;        
    }

    void Update()
    {
        aimRay.origin = transform.TransformPoint(barrelTip);
        aimRay.direction = transform.forward;

        Physics.Raycast(aimRay, out rayHitInfo);
    }
}
