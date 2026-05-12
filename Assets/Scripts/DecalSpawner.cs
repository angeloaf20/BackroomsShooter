using UnityEngine;

public class BulletDecalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bulletDecalPrefab;

    private void Start()
    {
        HitscanBullet bullet = GetComponent<HitscanBullet>();
        bullet.OnBulletHit += SpawnDecal;
    }

    public void SpawnDecal(GunshotResult result)
    {
        // TODO: 
        // - Spawn decals with memory pool
        // - Generate decal gameobject programatically. Attach URP DecalProjector to GO
        // - Acquire correct decal material based on the object hit
        if (result.Hit.transform.CompareTag("Enemy") || result.Hit.transform.CompareTag("Player")) 
        { 
            return; 
        }

        GameObject decal = Instantiate(_bulletDecalPrefab, result.Hit.point, Quaternion.LookRotation(-result.Hit.normal));
        Destroy(decal, 1.5f);
    }
}
