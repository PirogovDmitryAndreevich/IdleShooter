using UnityEngine;

public class ObstacleView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private HitFlash _hitEffect;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private DamageCollider _damageCollider;

    public HealthBar HealthBar =>
        _healthBar;
    public DamageableCollider DamageableCollider =>
        _damageableCollider;
    public HitFlash HitFlash =>
        _hitEffect;
    public DamageCollider DamageCollider =>
        _damageCollider;

    public void OnDied() =>
        gameObject.SetActive(false);

    public void OnReset() =>
        gameObject.SetActive(true);
}
