using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveInput;
    [SerializeField] private InputActionReference _jumpInput;
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _speed;

    public UnityEvent MoveStartEvent;
    public UnityEvent MoveEndEvent;

    private CharacterController _characterController;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _moveInput.action.performed += OnMovePerformed;
        _moveInput.action.canceled += OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        MoveStartEvent.Invoke();
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        MoveEndEvent.Invoke();
    }

    void Update()
    {
        Vector2 movement = _moveInput.action.ReadValue<Vector2>();
        Vector3 forwardMovement = _orientation.forward * movement.y;
        Vector3 rightMovement = _orientation.right * movement.x;

        Vector3 totalMovement = (forwardMovement + rightMovement).normalized * _speed;

        _characterController.SimpleMove(totalMovement);
    }
}
