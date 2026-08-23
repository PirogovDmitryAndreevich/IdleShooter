using System;
using UnityEngine;

public class TargetView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private HitFlash _hitEffect;

    public HealthBar HealthBar =>
        _healthBar;
    public DamageableCollider DamageableCollider =>
        _damageableCollider;

    public HitFlash HitFlash =>
        _hitEffect;

    public void OnDied() => 
        gameObject.SetActive(false);

    public void OnReset() => 
        gameObject.SetActive(true);
}
