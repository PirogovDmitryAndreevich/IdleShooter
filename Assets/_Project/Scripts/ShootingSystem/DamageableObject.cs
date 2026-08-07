using System;
using UnityEngine;

public class DamageableObject : BaseDamageable
{
    //[Header("DamageableObject Settings")]
    //
    [Header("Destroy Settings")]
    [SerializeField] private bool _isDestroyAfterDeath = false;

    [SerializeField] [Min(0.1f)]
    private float _destroyDelay = 2f;

    [Header("Debug Settings")]
    [SerializeField] private bool _isViewDebug = false;

    protected override void TakeDamageCore(float damage)
    {
        TakeDamage(damage, transform.position, Vector3.up);
    }

    protected override void TakeDamageCore(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        CurrentHealth = (int)Mathf.Max(CurrentHealth - damage, 0);

        if (_isViewDebug)
            Debug.Log($"{gameObject.name} получил {damage} урона. Здоровье {CurrentHealth}/{MaxHealth}");

        if (CurrentHealth <= 0)
            Died();
    }

    private void Died()
    {
        if (_isViewDebug)
            Debug.Log($"{gameObject.name} уничтожен/погиб");

        if (_isDestroyAfterDeath)
            Destroy(gameObject, _destroyDelay);
    }
}