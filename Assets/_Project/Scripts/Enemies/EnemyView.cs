using UnityEngine;

public abstract class EnemyView : MonoBehaviour
{
    [SerializeField] protected EnemiesAnimator Animator;    
    [SerializeField] private HitFlash _hitEffect;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private SoundsHandler _soundsHandler;

    public HitFlash HitEffect => _hitEffect;    

    public DamageableCollider DamageableCollider =>
        _damageableCollider;

    public HealthBar HealthBar =>
        _healthBar;

    public SoundsHandler SoundsHandler =>
        _soundsHandler;
}