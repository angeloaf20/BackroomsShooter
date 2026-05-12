using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private InputActionReference _shootAction;
    [SerializeField] private InputActionReference _reloadAction;
    [SerializeField] protected GunData _gunData;
    [SerializeField] private float _recoilSpeed;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private Vector3 _knockback;
    [SerializeField] private ParticleSystem _muzzleFlash;

    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _currentMagCount;

    private GunRecoiler _recoiler;
    [SerializeField] private bool _isShooting;
    [SerializeField] private bool _canShoot;
    private bool _isReloading;
    private bool _isRecoiling;
    private float _recoilRadius;
    private Vector3 _initialPosition;
    private Vector3 _velocity;
    private Transform _camOrigin;
    private Transform _barrelEnd;

    public Action OnGunTriggerPull;
    public Action OnGunShoot;

    private void Start()
    {
        _reloadAction.action.performed += OnReloadPerformed;
        _initialPosition = transform.localPosition;
        _currentMagCount = _gunData.MaxMagCount;
        _currentAmmo = _gunData.MaxAmmo;
        _camOrigin = Camera.main.transform;
        _barrelEnd = transform.Find("Barrel");
        _canShoot = true;
    }

    private void OnReloadPerformed(InputAction.CallbackContext obj)
    {
        if (_isShooting || _currentMagCount <= 0) return;

        StartCoroutine(nameof(DoReload));
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

    private void Fire()
    {
        if (!_canShoot || _currentAmmo <= 0) return;

        if (_gunData.IsAutomatic)
        {
            StartCoroutine(nameof(AutomaticShoot));
        }
        else
        {
            _canShoot = false;

            if (Physics.Raycast(_camOrigin.position, _camOrigin.forward, out RaycastHit hit, float.MaxValue))
            {
                if (hit.transform.TryGetComponent<Damageable>(out var comp))
                {
                    comp.Damage((int)_gunData.BaseDamage);
                }
            }

            _currentAmmo--;
        }
    }

    private void Release()
    {
        _canShoot = true;
    }

    IEnumerator AutomaticShoot()
    {
        _canShoot = false;
        _isShooting = true;

        if (Physics.Raycast(_camOrigin.position, _camOrigin.forward, out RaycastHit hit, float.MaxValue))
        {
            if (hit.transform.TryGetComponent<Damageable>(out var comp))
            {
                comp.Damage((int)_gunData.BaseDamage);
            }
        }

        _currentAmmo--;
        yield return new WaitForSeconds(_gunData.FireRate);
        _canShoot = true;
        _isShooting = false;
    }

    private IEnumerator DoReload()
    {
        _canShoot = false;
        _currentAmmo = _gunData.MaxAmmo;
        _currentMagCount--;
        yield return new WaitForSeconds(_gunData.FireRate);
        _canShoot = true;
    }
}
