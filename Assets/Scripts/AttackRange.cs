using UnityEngine;
using UnityEngine.Events;

public class AttackRange : MonoBehaviour
{
    public UnityEvent OnAttack;
    public UnityEvent OnDisarm;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnAttack?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnDisarm?.Invoke();
    }
}
