using System;
using UnityEngine;

public abstract class BaseDamageable : MonoBehaviour
{
    [Header("BaseDamageable Settings")]
    [SerializeField] protected int MaxHealth;

    protected int CurrentHealth;

    public event Action<int> HealthChanged;
    public event Action Death;

    public void TakeDamage(float damage)
    {
        TakeDamageCore(damage);
        InvokeEvents();
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        TakeDamageCore(damage, hitPoint, hitNormal);
        InvokeEvents();
    }

    protected abstract void TakeDamageCore(float damage);

    protected abstract void TakeDamageCore(float damage, Vector3 hitPoint, Vector3 hitNormal);

    private void InvokeEvents()
    {
        HealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
            Death?.Invoke();
    }
}
