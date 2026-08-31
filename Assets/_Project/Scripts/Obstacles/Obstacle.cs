using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private HitFlash _hitEffect;
    [SerializeField] private DamageableCollider _damageableCollider;

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