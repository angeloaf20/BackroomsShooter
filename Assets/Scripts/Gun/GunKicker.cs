using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GunKicker : MonoBehaviour
{
    [SerializeField] private float _recoilSpeed;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private Vector3 _knockback;

    private Gun _gun;
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private Vector3 _currentPosition;
    private bool _reached = false;

    private void Awake()
    {
        _gun = GetComponent<Gun>();
        _initialPosition = transform.localPosition;
    }

    private void Start()
    {
        _gun.OnRecoil += Kick;
    }

    private void Update()
    {
        _targetPosition = Vector3.Lerp(_targetPosition, _initialPosition, Time.deltaTime * _recoilSpeed);

        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * _recoilSpeed); ;
    }

    void Kick()
    {
        _targetPosition += new Vector3()
        {
            x = 0f,
            y = _initialPosition.y + _knockback.y,
            z = _initialPosition.z + _knockback.z,
        };
    }

}

