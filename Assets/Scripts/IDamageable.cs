using UnityEngine;
using UnityEngine.Events;

public abstract class Damageable : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private UnityEvent<int> OnHealthChange;

    private int _currentHealth;

    protected abstract void Kill();

    private void Start()
    {
        _currentHealth = _maxHealth;
        OnHealthChange?.Invoke(_currentHealth);
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        OnHealthChange?.Invoke(_currentHealth);

        if (_currentHealth <= 0f)
        {
            Kill();
        }
    }
}
