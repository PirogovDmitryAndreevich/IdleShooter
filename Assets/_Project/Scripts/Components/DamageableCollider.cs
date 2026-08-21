using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageableCollider : MonoBehaviour, IDamageable
{
    [SerializeField] private Collider _collider;

    public event Action<float, Vector3, Vector3> DamageReceived;

    public void TakeDamage(float damage) =>
        TakeDamage(damage, transform.position, Vector3.up);

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) =>
        DamageReceived?.Invoke(damage, hitPoint, hitNormal);
}