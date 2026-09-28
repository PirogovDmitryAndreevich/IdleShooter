using System;
using UnityEngine;

public class Health
{
    private float _currentHealth;
    private int _maxHealth;

    public event Action HealthChanged;
    public event Action Died;

    public Health(int maxHealth)
    {
        _maxHealth = maxHealth;

        MaxHealth = _maxHealth;
        _currentHealth = _maxHealth;
    }

    public virtual int MaxHealth { get; }
    public float CurrentHealth => _currentHealth;

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Clamp(
            _currentHealth - damage,
            0f,
            _maxHealth);

        HealthChanged?.Invoke();

        if (_currentHealth <= 0f)
            Death();
    }

    public void Heal(float heal)
    {
        _currentHealth = Mathf.Clamp(
            _currentHealth + heal,
            0f,
            _maxHealth);

        HealthChanged?.Invoke();
    }

    public void Reset()
    {
        _currentHealth = _maxHealth;
        HealthChanged?.Invoke();
    }

    private void Death()
    {
        Died?.Invoke();
    }

    protected void IncreaseMaxHealth(int value)
    {
        if (value <= 0f)
            return;

        _maxHealth += value;

        HealthChanged?.Invoke();
    }
}
