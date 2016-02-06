using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Magazine : Equipment
{
    [SerializeField]
    private MagazineData magazineData;
    public MagazineData MagazineData
    {
        get { return magazineData; }
    }

    public override ItemData ItemData
    {
        get { return magazineData; }
        protected set { magazineData = value as MagazineData; }
    }

    public string Caliber { get { return magazineData.Caliber; } }
    public int Capacity { get { return magazineData.Capacity; } }

    public AmmoData CurrentAmmo
    {
        get
        {
            return magazineData.CurrentAmmo;
        }
        set
        {
            magazineData.CurrentAmmo = value;
        }
    }

    public int CurrentAmmoCount
    {
        get
        {
            return magazineData.CurrentAmmoCount;
        }
        set
        {
            magazineData.CurrentAmmoCount = value;
        }
    }

    [SerializeField]
    private Animator animator;
    public Animator Animator
    {
        get { return animator; }
    }

    public static Magazine Create(MagazineData magazineData)
    {
        MagazineDatabase database = magazineData.Database as MagazineDatabase;

        string path = database.PrefabsPath + database.Find(x => x.ItemName == magazineData.ItemName).PrefabName;

        GameObject magazineGameObject = Instantiate(Resources.Load<GameObject>(path));

        return magazineGameObject.GetComponent<Magazine>();
    }
}
