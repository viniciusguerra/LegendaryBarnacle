using UnityEngine;
using System.Collections;

public class Firearm : MonoBehaviour
{
    public Ammo chamberedAmmo;
    public Magazine currentMagazine;
    public bool safetyOn;
    public bool cocked;
    public bool slideBack;

    public Character wielder;
    public Vector3 barrelTip;
    private Ray aimRay;
    private RaycastHit rayHitInfo;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private TrailRenderer trailRenderer;

    private string a_trigger = "TriggerPull";
    private string a_magRelease = "MagRelease";
    private string a_cock = "ToggleHammer";
    private string a_safety = "ToggleSafety";
    private string a_slide = "ToggleSlide";
    private string a_hasAmmo = "HasAmmoInChamber";
    private string a_safetyOn = "SafetyOn";    

    public void ToggleSafety()
    {
        safetyOn = !safetyOn;
        animator.SetTrigger(a_safety);
        animator.SetBool(a_safetyOn, safetyOn);
    }

    public void Cock()
    {
        if (!safetyOn)
        {
            if (!cocked)
            {
                SetHammerCocked(true);
            }
        }
    }

    public void ReleaseChamberedRound()
    {
        chamberedAmmo = null;
    }

    public void PullSlide()
    {
        if(!safetyOn && !slideBack)
        {
            animator.SetTrigger(a_slide);
            slideBack = true;
            
            SetHammerCocked(true);

            if (chamberedAmmo != null)
                ReleaseChamberedRound();
        }
    }

    public void ReleaseSlide()
    {
        if (slideBack)
        {
            animator.SetTrigger(a_slide);
            slideBack = false;

            LoadChamber();
        }
    }

    private void LoadChamber()
    {
        if (currentMagazine == null)
            return;
            
        currentMagazine.Feed(out chamberedAmmo);
        animator.SetBool(a_hasAmmo, chamberedAmmo == null ? false : true);
    }

    public void SetHammerCocked(bool cocked)
    {
        if (this.cocked != cocked)
        {
            this.cocked = cocked;
            animator.SetTrigger(a_cock);
        }
    }

    public void PullTrigger()
    {
        if (!safetyOn)
        {
            if (cocked)
            {
                if (!slideBack)
                {
                    if (chamberedAmmo != null)
                    {
                        animator.SetTrigger(a_trigger);
                        Shoot();
                    }
                    else
                    {
                        SetHammerCocked(false);
                    }
                }
            }
        }
    }

    private void Shoot()
    {
        Vector3 trailDirection = transform.forward * 100;

        if (rayHitInfo.collider != null)
        {
            trailDirection = rayHitInfo.point - transform.TransformPoint(barrelTip);
            Health hitObject = rayHitInfo.collider.GetComponent<Health>();

            if (hitObject != null)
            {
                hitObject.Damage(chamberedAmmo);
            }
        }

        DrawTrail(trailDirection);

        ReleaseChamberedRound();

        LoadChamber();        
    }

    private void DrawTrail(Vector3 direction)
    {
        trailRenderer.transform.position = transform.TransformPoint(barrelTip);
        trailRenderer.enabled = true;
        trailRenderer.transform.parent = null;

        Vector3 targetPosition = trailRenderer.transform.position + direction;

        iTween.MoveTo(trailRenderer.gameObject, iTween.Hash("position", targetPosition, "speed", 300, "oncompletetarget", gameObject, "oncomplete", "DrawTrailEnd"));
    }

    public void DrawTrailEnd()
    {
        trailRenderer.transform.parent = transform;
        trailRenderer.Clear();
        trailRenderer.enabled = false;
    }

    void Start()
    {
        aimRay = new Ray();
        trailRenderer.GetComponentInChildren<TrailRenderer>();
        trailRenderer.enabled = false;
    }

    void Update()
    {
        aimRay.origin = transform.TransformPoint(barrelTip);
        aimRay.direction = transform.forward;

        Physics.Raycast(aimRay, out rayHitInfo);
    }
}
