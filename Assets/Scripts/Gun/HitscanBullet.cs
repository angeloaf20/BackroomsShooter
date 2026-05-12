using System;
using UnityEngine;

[RequireComponent(typeof(BulletDecalSpawner))]
public class HitscanBullet : MonoBehaviour, IBullet
{
    public event Action<GunshotResult> OnBulletHit;
    public event Action<Transform> OnBulletNotHit;
    private BulletDecalSpawner _decalSpawner;

    private void Awake()
    {
        _decalSpawner = GetComponent<BulletDecalSpawner>();
    }

    private Vector3 GetBulletDirection(Vector3 origin, float radiusStrength)
    {
        Quaternion rotation = Quaternion.Euler(UnityEngine.Random.insideUnitSphere * radiusStrength);
        Vector3 result = rotation * origin;
        return result.normalized;
    }

    public void DetectAndDoDamage(Transform origin, GunData gunData, float radiusStrength)
    {
        if (Physics.Raycast(origin.position, GetBulletDirection(origin.forward, radiusStrength), out RaycastHit hit, float.MaxValue))
        {
            OnBulletHit?.Invoke(new GunshotResult()
            {
                Damage = gunData.BaseDamage,
                BarrelEnd = gunData.Prefab.transform.Find("Barrel"),
                Hit = hit
            });

            if (hit.transform.TryGetComponent<Damageable>(out var comp))
            {
                comp.Damage((int)gunData.BaseDamage);
            }
        }
        else
        {
            OnBulletNotHit?.Invoke(gunData.Prefab.transform.Find("Barrel"));
        }
    }
}
