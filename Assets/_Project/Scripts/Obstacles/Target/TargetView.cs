using System;
using UnityEngine;

public class TargetView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private DamageableCollider _damageableCollider;

    public HealthBar HealthBar =>
        _healthBar;
    public DamageableCollider DamageableCollider =>
        _damageableCollider;

    public void OnDied() => 
        gameObject.SetActive(false);

    public void OnReset() => 
        gameObject.SetActive(true);
}
