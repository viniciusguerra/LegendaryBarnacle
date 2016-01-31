using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[Serializable]
public class Firearm : Equipment
{
    [SerializeField]
    private FirearmData firearmData;
    public override ItemData ItemData { get { return firearmData; } protected set { firearmData = value as FirearmData; } }

    public string Caliber { get { return firearmData.Caliber; } }
    public string FirearmType { get { return firearmData.FirearmType; } }
    public MagazineData CurrentMagazine
    {
        get { return firearmData.CurrentMagazine; }
        set
        {
            if (value != null && value.Caliber == firearmData.Caliber)
            {
                firearmData.CurrentMagazine = value;                
            }
            else
                firearmData.CurrentMagazine = null;
        }
    }    
    public AmmoData ChamberedAmmo
    {
        get { return firearmData.ChamberedAmmo; }
        set
        {
            if (value != null && value.caliber == firearmData.Caliber)
            {
                firearmData.ChamberedAmmo = value;
                chamberedAmmoPrefab = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(ChamberedAmmo.Database.PrefabsPath + ChamberedAmmo.prefabName + ".prefab"));
                chamberedAmmoPrefab.transform.position = chamberedAmmoTransform.position;
                chamberedAmmoPrefab.transform.rotation = chamberedAmmoTransform.rotation;
                chamberedAmmoPrefab.transform.parent = chamberedAmmoTransform;
                chamberedAmmoPrefab.name = "Chambered " + ChamberedAmmo.ammoName;
            }
            else
                firearmData.ChamberedAmmo = null;
        }
    }

    protected GameObject chamberedAmmoPrefab;
    public bool safetyOn;
    public bool cocked;
    public bool slideBack;

    public Character wielder;
    public Transform barrelTipTransform;
    public Transform chamberedAmmoTransform;    
    protected Ray aimRay;
    protected RaycastHit rayHitInfo;

    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected TrailRenderer trailRenderer;

    protected string a_trigger = "TriggerPull";
    protected string a_magRelease = "MagRelease";
    protected string a_cock = "ToggleHammer";
    protected string a_safety = "ToggleSafety";
    protected string a_hasAmmo = "HasAmmoInChamber";
    protected string a_safetyOn = "SafetyOn";
    protected string a_slideBack = "SlideBack";

    public void ToggleSafety()
    {
        if (!slideBack)
        {
            safetyOn = !safetyOn;
            animator.SetTrigger(a_safety);
            animator.SetBool(a_safetyOn, safetyOn);
        }
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
        if (chamberedAmmoPrefab != null)
        {
            chamberedAmmoPrefab.name = "Released " + ChamberedAmmo.ammoName;

            ChamberedAmmo = null;

            chamberedAmmoPrefab.transform.parent = null;
            Rigidbody roundRigidbody = chamberedAmmoPrefab.GetComponent<Rigidbody>();
            roundRigidbody.isKinematic = false;
            roundRigidbody.AddForce(new Vector3(1, 1.4f, 0) * 2, ForceMode.Impulse);

            chamberedAmmoPrefab = null;
        }
    }

    public void PullSlide()
    {
        if(!safetyOn && !slideBack)
        {
            slideBack = true;
            animator.SetBool(a_slideBack, slideBack);

            SetHammerCocked(true);

            if (ChamberedAmmo != null)
                ReleaseChamberedRound();
        }
    }

    public void ReleaseSlide()
    {
        if (slideBack)
        {
            slideBack = false;
            animator.SetBool(a_slideBack, slideBack);

            LoadChamber();
        }
    }

    /// <summary>
    /// Loads given magazine, if caliber is compatible, and returns the remaining one, if available. If caliber is not compatible, returns the given magazine.
    /// </summary>
    /// <param name="magazineData">Magazine to be loaded</param>
    /// <returns>Remaining magazine</returns>
    public MagazineData LoadMagazine(MagazineData magazineData)
    {
        if (magazineData == null || magazineData.Caliber == firearmData.Caliber)
        {
            MagazineData oldMagazine = CurrentMagazine;

            CurrentMagazine = magazineData;

            return oldMagazine;
        }
        else
            return magazineData;
    }

    private void LoadChamber()
    {
        if (CurrentMagazine == null)
            return;
            
        ChamberedAmmo = CurrentMagazine.Feed();
        animator.SetBool(a_hasAmmo, ChamberedAmmo == null ? false : true);
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
                    if (ChamberedAmmo != null)
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
            trailDirection = rayHitInfo.point - barrelTipTransform.position;
            Health hitObject = rayHitInfo.collider.GetComponent<Health>();

            if (hitObject != null)
            {
                hitObject.Damage(ChamberedAmmo);
            }
        }

        DrawTrail(trailDirection);

        StartCoroutine(SlideOnShoot());
    }

    private void DrawTrail(Vector3 direction)
    {
        trailRenderer.transform.position = barrelTipTransform.position;
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

    private IEnumerator SlideOnShoot()
    {
        PullSlide();

        yield return new WaitForEndOfFrame();

        if (CurrentMagazine != null && CurrentMagazine.CurrentAmmoCount > 0)
            ReleaseSlide();
    }

    void Start()
    {
        aimRay = new Ray();
        trailRenderer.GetComponentInChildren<TrailRenderer>();
        trailRenderer.enabled = false;

        if(ChamberedAmmo != null && chamberedAmmoPrefab == null)
        {
            chamberedAmmoPrefab = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(ChamberedAmmo.Database.PrefabsPath + ChamberedAmmo.prefabName + ".prefab"));
            chamberedAmmoPrefab.transform.position = chamberedAmmoTransform.position;
            chamberedAmmoPrefab.transform.rotation = chamberedAmmoTransform.rotation;
            chamberedAmmoPrefab.transform.parent = chamberedAmmoTransform;
            chamberedAmmoPrefab.name = "Chambered " + ChamberedAmmo.ammoName;
        }
    }

    void Update()
    {
        aimRay.origin = barrelTipTransform.position;
        aimRay.direction = transform.forward;

        Physics.Raycast(aimRay, out rayHitInfo);
    }
}
