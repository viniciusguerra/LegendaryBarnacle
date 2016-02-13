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
        set { magazineData = value; }
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

    public static Magazine CreateMagazine(MagazineData data)
    {
        MagazineDatabase database = data.Database as MagazineDatabase;

        string path = database.PrefabsPath + data.PrefabName;

        GameObject magazineGameObject = Instantiate(Resources.Load<GameObject>(path));

        Magazine magazine = magazineGameObject.GetComponent<Magazine>();

        magazine.magazineData = data;

        return magazine;
    }

    private void UpdateBulletRenderers()
    {
        for (int i = 1; i <= magazineData.capacity; i++)
        {
            bool isBulletVisible = i <= CurrentAmmoCount;
            transform.FindChild("Bullet" + i).GetComponent<MeshRenderer>().enabled = isBulletVisible ? true : false;
        }
    }

    public AmmoData Feed()
    {
        AmmoData ammo = MagazineData.Feed();
        UpdateBulletRenderers();

        return ammo;
    }

    public StackData[] Load(AmmoData ammo, int amount)
    {
        StackData[] stack = MagazineData.Load(ammo, amount);
        UpdateBulletRenderers();

        return stack;
    }
}