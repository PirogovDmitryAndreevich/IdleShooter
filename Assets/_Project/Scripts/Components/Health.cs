using System;
using UnityEngine;

public class Health
{
    private readonly int _maxHealth;

    private float _currentHealth;

    public event Action HealthChanged;

    public Health(int maxHealth)
    {
        _maxHealth = maxHealth;

        _currentHealth = _maxHealth;
    }

    public int MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Clamp(
            _currentHealth - damage,
            0f,
            _maxHealth);

        HealthChanged?.Invoke();
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
}