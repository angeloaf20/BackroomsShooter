using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSway : MonoBehaviour
{
    [SerializeField] private InputActionReference _lookAction;
    [SerializeField] private float _swayMultiplier;
    [SerializeField] private float _smooth;

    private void Update()
    {
        Vector2 lookInput = _swayMultiplier * _lookAction.action.ReadValue<Vector2>();

        Quaternion rotationX = Quaternion.AngleAxis(-lookInput.y, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(lookInput.x, Vector3.up);

        Quaternion result = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, result, _smooth * Time.deltaTime);
    }
}
