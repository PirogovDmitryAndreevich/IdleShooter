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

    public int MaxHealth =>
        _maxHealth;

    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0f, _maxHealth);
            HealthChanged?.Invoke();
        }
    }

    public void TakeDamage(float damage) =>
        CurrentHealth -= damage;

    public void Heal(float heal) =>
        CurrentHealth += heal;

    public void Reset() =>
        CurrentHealth = _maxHealth;
}