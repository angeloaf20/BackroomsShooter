using Assets.Scripts.Gun;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

[System.Serializable]
public struct GunshotResult
{
    public Transform BarrelEnd;
    public RaycastHit Hit;
    public float Damage;
    public Vector3 Direction;
}

public class Gun : MonoBehaviour
{
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private InputActionReference _reloadAction;
    [SerializeField] protected GunData _gunData;
    [SerializeField] private GameObject _bulletDecalPrefab;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _currentMagCount;
    [SerializeField] private Transform _orientation;
    [SerializeField] private IFPSHands _hands;
    [SerializeField] private UnityEvent<int> _onAmmoChange;
    [SerializeField] private UnityEvent<int> _onMagChange;

    private bool _isShooting;
    private bool _canShoot;
    [SerializeField] private bool _firstShot = true;
    private CinemachineImpulseSource _recoilSource;

    public Action OnGunTriggerPull;
    public Action OnRecoil;
    public Action<GunshotResult> OnShoot;

    private void Awake()
    {
        _recoilSource =GetComponent<CinemachineImpulseSource>();
        _hands = GetComponent<IFPSHands>();
    }

    private void Start()
    {
        _hands.MoveHands(_gunData);
        _currentMagCount = _gunData.MaxMagCount;
        _currentAmmo = _gunData.MaxAmmo;
        _canShoot = true;
        _reloadAction.action.performed += OnReloadPerformed;
        _onAmmoChange.Invoke(_currentAmmo);
        _onMagChange.Invoke(_currentMagCount);
    }

    private void Update()
    {
        if (_shootAction.action.IsPressed())
        {
            Fire();
        }

        if (_shootAction.action.WasReleasedThisFrame())
        {
            Release();
        }
    }

    private void OnReloadPerformed(InputAction.CallbackContext obj)
    {
        if (_isShooting || _currentMagCount <= 0) return;

        StartCoroutine(nameof(DoReload));
    }

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (!_canShoot || _currentAmmo <= 0) return;

        _recoilSource.GenerateImpulse();

        OnRecoil?.Invoke();
        

        StopCoroutine(nameof(ResetFirstShot));

        if (_gunData.IsAutomatic)
        {
            StartCoroutine(nameof(AutomaticShoot));
        }
        else
        {
            _canShoot = false;
            _muzzleFlash.Play();
            

            if (Physics.Raycast(_orientation.position, RotateDirection(), out RaycastHit hit, float.MaxValue))
            {
                if (hit.transform.TryGetComponent<Damageable>(out var comp))
                {
                    comp.Damage((int)_gunData.BaseDamage);
                }
                //OnShootHit.Invoke(hit);
            }
            else
            {
                //OnShoot.Invoke();
            }


            StartCoroutine(nameof(SemiMuzzleFlash));
            SpawnDecal(hit);
            OnShoot.Invoke(new GunshotResult()
            {

            });

            _currentAmmo--;
            _onAmmoChange?.Invoke(_currentAmmo);
        }
    }

    public void Release()
    {
        _canShoot = true;
        _isShooting = false;
        _muzzleFlash.Stop();
        StartCoroutine(nameof(ResetFirstShot));
    }

    public void Reload()
    {
        if (_isShooting || _currentMagCount <= 0) return;

        StartCoroutine(nameof(DoReload));
    }

    private IEnumerator ResetFirstShot()
    {
        yield return new WaitForSeconds(0.75f);
        _firstShot = true;
    }

    private IEnumerator AutomaticShoot()
    {
        _canShoot = false;
        _isShooting = true;
        _muzzleFlash.Play();

        Vector3 dir = RotateDirection();

        bool didHit = Physics.Raycast(_orientation.position, dir, out RaycastHit hitInfo, float.MaxValue);

        _currentAmmo--;
        OnShoot.Invoke(new GunshotResult()
        {
            Hit = hitInfo,
            Direction = dir,
            Damage = _gunData.BaseDamage
        });

        if (didHit)
        {
            if (hitInfo.transform.TryGetComponent<Damageable>(out var comp))
            {
                comp.Damage((int)_gunData.BaseDamage);
            }
            SpawnDecal(hitInfo);
        }
        else
        {
            //OnShoot.Invoke();
        }

        _onAmmoChange.Invoke(_currentAmmo);
        yield return new WaitForSeconds(_gunData.FireRate);
        _muzzleFlash.Stop();
        _canShoot = true;
        _isShooting = false;
    }

    private IEnumerator DoReload()
    {
        _canShoot = false;
        _currentAmmo = _gunData.MaxAmmo;
        _currentMagCount--;
        yield return new WaitForSeconds(_gunData.FireRate);
        _onAmmoChange.Invoke(_currentAmmo);
        _onMagChange.Invoke(_currentMagCount);
        _canShoot = true;
    }

    private IEnumerator SemiMuzzleFlash()
    {
        yield return new WaitForSeconds(0.075f);
        _muzzleFlash.Stop();
    }

    public void SpawnDecal(RaycastHit hit)
    {
        // TODO: 
        // - Spawn decals with memory pool
        // - Generate decal gameobject programatically. Attach URP DecalProjector to GO
        // - Acquire correct decal material based on the object hit
        if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Player"))
        {
            return;
        }

        GameObject decal = Instantiate(_bulletDecalPrefab, hit.point, Quaternion.LookRotation(-hit.normal));
        Destroy(decal, 1.5f);
    }

    private Vector3 RotateDirection()
    {
        if (_firstShot)
        {
            _firstShot = false;
            return _orientation.forward;
        }

        return Quaternion.Euler(
            Random.Range(-_gunData.RecoilRange.x, _gunData.RecoilRange.x),
            Random.Range(-_gunData.RecoilRange.y, _gunData.RecoilRange.y),
            Random.Range(-_gunData.RecoilRange.z, _gunData.RecoilRange.z)
        ) * _orientation.forward;
    }
}