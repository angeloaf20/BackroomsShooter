using System.Collections;
using System.Threading;
using UnityEngine;

public class GunshotTrail : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private Transform _barrelEnd;
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _speed;

    private Gun _gun;

    private void Awake()
    {
        _gun = GetComponent<Gun>();
        _gun.OnShoot += SpawnTrail;
    }

    public void SpawnTrail(GunshotResult hit)
    {
        // TODO: 
        // - Spawn trails with memory pool
        TrailRenderer trail = Instantiate(_trail, _barrelEnd.position, Quaternion.identity);
        StartCoroutine(DoSpawnTrail(trail, hit.Direction));
    }

    IEnumerator DoSpawnTrail(TrailRenderer trail, Vector3 direction)
    {
        float time = 0f;

        while (time < 1f)
        {
            trail.transform.Translate(direction * Time.deltaTime/trail.time * _speed);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        Destroy(trail.gameObject);
    }
}
