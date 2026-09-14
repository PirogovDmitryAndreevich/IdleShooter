using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] protected EnemiesAnimator Animator;    
    [SerializeField] private HitFlash _hitEffect;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private HealthBar _healthBar;

    public HitFlash HitEffect => _hitEffect;    

    public DamageableCollider DamageableCollider =>
        _damageableCollider;

    public HealthBar HealthBar =>
        _healthBar;
}