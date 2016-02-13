using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Firearm : Equipment
{
    #region Properties
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
        
    //Operation bools
    [SerializeField]
    protected bool safetyOn;
    public bool SafetyOn { get { return safetyOn; } }
    [SerializeField]
    protected bool cocked;
    public bool Cocked { get { return cocked; } }
    [SerializeField]
    protected bool slideFullBack;
    public bool SlideFullBack { get { return slideFullBack; } }
    [SerializeField]
    protected bool slideHalfBack;
    public bool SlideHalfBack { get { return slideHalfBack; } }
    [SerializeField]
    protected bool magazineIsAttached;
    public bool MagazineIsAttached { get { return magazineIsAttached; } }

    [SerializeField]
    protected GameObject chamberedAmmoPrefab;

    //Wielder
    public Character wielder;        

    //Transforms
    [SerializeField]
    private Transform weaponCameraOperationTransform;
    public Transform OperationTransform { get { return weaponCameraOperationTransform; } }
    [SerializeField]
    private Transform weaponCameraChamberTransform;
    public Transform ChamberTransform { get { return weaponCameraChamberTransform; } }
    [SerializeField]
    private Transform weaponCameraMagazineTransform;
    public Transform MagazineTransform { get { return weaponCameraMagazineTransform; } }
    public Transform barrelTipTransform;
    public Transform chamberedAmmoTransform;
    public Transform loadedMagazineTransform;

    //Aiming Ray
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
    protected readonly int a_slideLayer = 2;
    protected readonly string a_slideHalfBack = "SlideHalfBack";
    protected readonly string a_releaseMagazine = "Release";

    //Magazine Game Object
    [SerializeField]
    public Magazine currentMagazine;

    private void CreateMagazine()
    {
        currentMagazine = CurrentMagazineData.CreateMagazine();
        currentMagazine.transform.SetParent(loadedMagazineTransform, false);
        currentMagazine.transform.localPosition = Vector3.zero;

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
    protected Animator magazineAnimator;
    protected readonly string a_magazineRelease = "Release";

    //Trail
    [SerializeField]
    protected TrailRenderer trailRenderer;    

    public void ToggleSafety()
    {
        if (!slideFullBack)
        {
            safetyOn = !safetyOn;
            firearmAnimator.SetTrigger(a_safety);
            firearmAnimator.SetBool(a_safetyOn, safetyOn);
        }
    }
    #endregion

    #region Methods
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
        if (ChamberedAmmo != null && !string.IsNullOrEmpty(ChamberedAmmo.ammoName) && chamberedAmmoPrefab != null)
        {
            chamberedAmmoPrefab.transform.SetParent(null);
            chamberedAmmoPrefab.name = "Released " + ChamberedAmmo.ammoName;
            Rigidbody roundRigidbody = chamberedAmmoPrefab.GetComponent<Rigidbody>();
            roundRigidbody.isKinematic = false;
            roundRigidbody.AddForce(new Vector3(1, 1.4f, 0) * roundRigidbody.mass, ForceMode.Impulse);

            ChamberedAmmo = null;
            chamberedAmmoPrefab = null;
        }
    }

    public void FullSlidePull()
    {
        if(!safetyOn && !slideFullBack)
        {
            slideFullBack = true;
            slideHalfBack = false;
            firearmAnimator.SetBool(a_slideBack, slideFullBack);

            SetHammerCocked(true);

            //Being called at end of animation by animation event
            //if (ChamberedAmmo != null)
            //    ReleaseChamberedRound();
        }
    }

    public void HalfSlidePull()
    {
        if (!safetyOn && !slideFullBack)
        {
            slideHalfBack = true;
            firearmAnimator.SetTrigger(a_slideHalfBack);

            SetHammerCocked(true);
        }
    }
    
    public void HalfSlideToggle()
    {
        if (!slideHalfBack)
            HalfSlidePull();
        else
            ReleaseSlide();
    }

    public void ReleaseSlide()
    {
        if (slideFullBack)
        {
            slideFullBack = false;
            firearmAnimator.SetBool(a_slideBack, slideFullBack);

            //Being called at end of animation by animation event
            //LoadChamber();
        }
        if(slideHalfBack)
        {            
            slideHalfBack = false;
            firearmAnimator.SetTrigger(a_slideHalfBack);
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

            if (magazineData != null)
            {
                magazineAnimator = currentMagazine.Animator;
                magazineIsAttached = true;
            }
            else
            {
                magazineIsAttached = false;
            }

            return oldMagazine;
        }
        else
        {
            return magazineData;
        }
    }

    public void ToggleCheckMagazine()
    {
        magazineIsAttached = !magazineIsAttached;
        magazineAnimator.SetBool(a_releaseMagazine, !magazineIsAttached);
    }

    private void LoadChamber()
    {
        if (CurrentMagazineData == null || magazineIsAttached == false || CurrentMagazineData.CurrentAmmoCount == 0)
            return;
            
        AmmoData fedAmmo = currentMagazine.Feed();

        ChamberedAmmo = fedAmmo;
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
                if (!slideFullBack && !slideHalfBack)
                {
                    firearmAnimator.SetTrigger(a_trigger);

                    SetHammerCocked(false);

                    //Shoot();
                    //Called at end of hammer release
                }
            }
        }
    }

    private void Shoot()
    {
        if (ChamberedAmmo == null || string.IsNullOrEmpty(ChamberedAmmo.ammoName))
            return;

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

        while (!firearmAnimator.GetCurrentAnimatorStateInfo(a_slideLayer).IsName(a_slideBack))
            yield return null;

        while (firearmAnimator.GetCurrentAnimatorStateInfo(a_slideLayer).normalizedTime < 1)
            yield return null;

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
            cocked = true;
        }        

        if(CurrentMagazineData != null)
        {
            if (currentMagazine == null)
            {
                CreateMagazine();
            }

            magazineIsAttached = true;
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
    #endregion
}
