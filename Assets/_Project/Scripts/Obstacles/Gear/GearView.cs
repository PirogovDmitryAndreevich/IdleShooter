using UnityEngine;

public class GearView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private DamageCollider _damageCollider;

    public HealthBar HealthBar =>
        _healthBar;
    public DamageableCollider DamageableCollider =>
        _damageableCollider;
    public DamageCollider DamageCollider =>
        _damageCollider;

    public void OnDied() =>
        gameObject.SetActive(false);

    public void OnReset() =>
        gameObject.SetActive(true);
}
