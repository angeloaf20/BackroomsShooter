using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GunBob : MonoBehaviour
{
   
    [SerializeField] private float moveSpeed;
    [SerializeField] private float returnSpeed;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float _runningFrequency;
    [SerializeField] private float _runningAmplitude;
    [SerializeField] private float _idleFrequency;
    [SerializeField] private float _idleAmplitude;

    private bool _isIdle = true;

    private float _currentFrequency;
    private float _currentAmplitude;

    private bool _isRunning;
    private Vector3 _initialPos;

    private void Start()
    {
        _initialPos = transform.localPosition;

        _currentAmplitude = _idleAmplitude;
        _currentFrequency = _idleFrequency;
    }

    public void SetStrength()
    {
        if (_isIdle)
        {
            _isIdle = false;
            _currentAmplitude = _runningAmplitude;
            _currentFrequency = _runningFrequency;
        }
        else
        {
            _isIdle = true;
            _currentAmplitude = _idleAmplitude;
            _currentFrequency = _idleFrequency;
        }
    }

    private void Update()
    {
        Vector3 target = _initialPos;

        target.x = (Mathf.Cos(Time.time / 2f * _currentFrequency) * _currentAmplitude) + offset.x;
        target.y = (Mathf.Sin(Time.time * _currentFrequency) * _currentAmplitude) + offset.y;

        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * moveSpeed);

        if (transform.localPosition == target)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPos, Time.deltaTime * returnSpeed);
        }
    }
}
