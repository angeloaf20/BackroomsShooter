using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/GunData")]
public class GunData : ScriptableObject
{
    public GameObject Prefab;
    public GameObject DecalPrefab;
    public float BaseDamage;
    public int MaxMagCount;
    public int MaxAmmo;
    public bool IsAutomatic;
    public float FireRate;
    public float ReloadTime;
    public Vector3 RecoilRange;
}
