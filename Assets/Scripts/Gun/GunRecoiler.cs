using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GunRecoiler : MonoBehaviour
{
    [SerializeField] private float _recoilSpeed;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private Vector3 _recoilRotation;

    private Gun _gun;
    private Vector3 _targetRotation;
    private Vector3 _currentRotation;

    private void Awake()
    {
        _gun = GetComponent<Gun>();
    }

    private void Start()
    {
        _gun.OnRecoil += Recoil; 
    }

    private void Update()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed *  Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _recoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void Recoil()
    {
        _targetRotation += new Vector3(
            _recoilRotation.x, 
            Random.Range(-_recoilRotation.y, _recoilRotation.y), 
            Random.Range(-_recoilRotation.z, _recoilRotation.z)
        );
    }
}
