using System;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;


public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private float _recoilSpeed;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private Vector3 _recoilRotation;
    [SerializeField] private Vector3 _recoilRange;
    [SerializeField] private ParticleSystem _muzzleFlash;
    public Transform Orientation;
    public GunData Gun;
    public bool IsShooting;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private Transform _barrelEnd;
    [SerializeField] private GameObject GunModel;
    [SerializeField] private float _trailSpeed;

    private float _currentCooldown;

    private int _ammoCount;

    private Vector3 _targetRotation;
    private Vector3 _currentRotation;


    private void Start()
    {
        _ammoCount = Gun.MaxAmmo;
    }

    private void Update()
    {
        if (IsShooting)
        {
            Debug.Log("Enemy shooting!");
            StartCoroutine(nameof(AutomaticShoot));
        }
        
        if (_ammoCount <= 0)
        {
            Reload();
            _muzzleFlash.Stop();
        }

        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _recoilSpeed * Time.deltaTime);
        GunModel.transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    private IEnumerator AutomaticShoot()
    {
        Vector3 dir = RotateDirection();
        IsShooting = false;

        bool didHit = Physics.Raycast(Orientation.position, dir, out RaycastHit hitInfo, float.MaxValue);

        _ammoCount--;

        Recoil();
        _muzzleFlash.Play();

        SpawnTrail(new GunshotResult()
        {
            Hit = hitInfo,
            BarrelEnd = _barrelEnd,
            Direction = dir
        });

        if (didHit)
        {
            if (hitInfo.transform.TryGetComponent<Damageable>(out var comp))
            {
                comp.Damage(5);
            }
            
        }
        else
        {
            //OnShoot.Invoke();
        }

        yield return new WaitForSeconds(Gun.FireRate);

        IsShooting = true;
        _muzzleFlash.Stop();
    }


    private Vector3 RotateDirection()
    {
        return Quaternion.Euler(
            Random.Range(-_recoilRange.x, _recoilRange.x),
            Random.Range(-_recoilRange.y, _recoilRange.y),
            Random.Range(-_recoilRange.z, _recoilRange.z)
        ) * Orientation.forward;
    }

    public void ToggleIsShooting()
    {
        IsShooting = !IsShooting;
    }

    private void Reload()
    {
        _ammoCount = Gun.MaxAmmo;
       
    }

    public void Recoil()
    {
        _targetRotation += new Vector3(
            _recoilRotation.x,
            Random.Range(-_recoilRotation.y, _recoilRotation.y),
            Random.Range(-_recoilRotation.z, _recoilRotation.z)
        );
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
            trail.transform.Translate(direction * Time.deltaTime / trail.time * _trailSpeed);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        Destroy(trail.gameObject);
    }
}
