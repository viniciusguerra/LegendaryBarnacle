using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Firearm : Equipment
{
    [SerializeField]
    private FirearmData firearmData;
    public override ItemData ItemData { get { return firearmData; } protected set { firearmData = value as FirearmData; } }

    public string Caliber { get { return firearmData.Caliber; } }
    public string FirearmType { get { return firearmData.FirearmType; } }
    public MagazineData CurrentMagazineData
    {
        get { return firearmData.CurrentMagazine; }
        set
        {
            if (value != null && value.Caliber == firearmData.Caliber)
            {
                firearmData.CurrentMagazine = value;

                CreateMagazine();
            }
            else
            {
                firearmData.CurrentMagazine = null;

                DestroyMagazine();
            }
        }
    }
    public Magazine currentMagazine;

    private void CreateMagazine()
    {
        currentMagazine = Magazine.Create(firearmData.CurrentMagazine);
        currentMagazine.transform.SetParent(transform, false);

        magazineAnimator = currentMagazine.Animator;
    }

    private void DestroyMagazine()
    {
        if (currentMagazine != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(currentMagazine.gameObject);
#else
            Destroy(currentMagazine.gameObject);
#endif

            magazineAnimator = null;

            currentMagazine = null;
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
                chamberedAmmoPrefab = Instantiate(Resources.Load<GameObject>(ChamberedAmmo.Database.PrefabsPath + ChamberedAmmo.prefabName));
                chamberedAmmoPrefab.transform.position = chamberedAmmoTransform.position;
                chamberedAmmoPrefab.transform.rotation = chamberedAmmoTransform.rotation;
                chamberedAmmoPrefab.transform.parent = chamberedAmmoTransform;
                chamberedAmmoPrefab.name = "Chambered " + ChamberedAmmo.ammoName;
            }
            else
                firearmData.ChamberedAmmo = null;
        }
    }
        
    public bool safetyOn;
    public bool cocked;
    public bool slideBack;
    public bool slideHalfBack;

    [SerializeField]
    protected GameObject chamberedAmmoPrefab;
    public Character wielder;
    public Transform barrelTipTransform;
    public Transform chamberedAmmoTransform;    
    protected Ray aimRay;
    protected RaycastHit rayHitInfo;

    [SerializeField]
    protected Animator firearmAnimator;
    protected readonly string a_trigger = "TriggerPull";
    protected readonly string a_cock = "ToggleHammer";
    protected readonly string a_safety = "ToggleSafety";
    protected readonly string a_hasAmmo = "HasAmmoInChamber";
    protected readonly string a_safetyOn = "SafetyOn";
    protected readonly string a_slideBack = "SlideBack";
    protected readonly string a_slideHalfBack = "SlideHalfBack";

    [SerializeField]
    protected Animator magazineAnimator;
    protected readonly string a_magazineRelease = "Release";

    [SerializeField]
    protected TrailRenderer trailRenderer;    

    public void ToggleSafety()
    {
        if (!slideBack)
        {
            safetyOn = !safetyOn;
            firearmAnimator.SetTrigger(a_safety);
            firearmAnimator.SetBool(a_safetyOn, safetyOn);
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

    public void FullSlidePull()
    {
        if(!safetyOn && !slideBack)
        {
            slideBack = true;
            firearmAnimator.SetBool(a_slideBack, slideBack);

            SetHammerCocked(true);

            if (ChamberedAmmo != null)
                ReleaseChamberedRound();
        }
    }
    
    public void HalfSlideToggle()
    {
        slideHalfBack = !slideHalfBack;
        firearmAnimator.SetBool(a_slideHalfBack, slideHalfBack);
    }

    public void FullSlideToggle()
    {
        if (slideBack || slideHalfBack)
            ReleaseSlide();
        else
            FullSlidePull();
    }

    public void ReleaseSlide()
    {
        if (slideBack)
        {
            slideBack = false;
            firearmAnimator.SetBool(a_slideBack, slideBack);

            LoadChamber();
        }
        if(slideHalfBack)
        {
            slideHalfBack = false;
            firearmAnimator.SetBool(a_slideHalfBack, slideHalfBack);
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
            MagazineData oldMagazine = CurrentMagazineData;

            CurrentMagazineData = magazineData;

            if(magazineData != null)
                magazineAnimator = currentMagazine.Animator;

            return oldMagazine;
        }
        else
        {
            return magazineData;
        }
    }

    private void LoadChamber()
    {
        if (CurrentMagazineData == null)
            return;
            
        ChamberedAmmo = CurrentMagazineData.Feed();
        firearmAnimator.SetBool(a_hasAmmo, ChamberedAmmo == null ? false : true);
    }

    public void SetHammerCocked(bool cocked)
    {
        if (this.cocked != cocked)
        {
            this.cocked = cocked;
            firearmAnimator.SetTrigger(a_cock);
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
                        firearmAnimator.SetTrigger(a_trigger);
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
                hitObject.ApplyDamage(ChamberedAmmo, barrelTipTransform);
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
        FullSlidePull();

        yield return new WaitForEndOfFrame();

        if (CurrentMagazineData != null && CurrentMagazineData.CurrentAmmoCount > 0)
            ReleaseSlide();
    }

    void Start()
    {
        aimRay = new Ray();
        trailRenderer.GetComponentInChildren<TrailRenderer>();
        trailRenderer.enabled = false;
        
        if(firearmData.ChamberedAmmo != null)
        {
            firearmAnimator.SetBool(a_hasAmmo, true);
        }

        if(!cocked)
        {
            SetHammerCocked(true);
        }

        if(ChamberedAmmo != null && chamberedAmmoPrefab == null)
        {            
            chamberedAmmoPrefab = Instantiate(Resources.Load<GameObject>(ChamberedAmmo.Database.PrefabsPath + ChamberedAmmo.prefabName));
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
