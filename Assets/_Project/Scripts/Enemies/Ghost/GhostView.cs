using UnityEngine;

public class GhostView : EnemyView
{
    [SerializeField] private AnimationEvent _attackEvent;
    [SerializeField] private MeleeDamageCollider _damageCollider;
    [SerializeField] private GhostBehavior _behaviour;

    public AnimationEvent AttackEvent =>
        _attackEvent;
    public GhostBehavior GhostBehaviour =>
        _behaviour;
    public MeleeDamageCollider DamageCollider =>
        _damageCollider;

    public void Idle()
    {
        _animator.Idle();
        _damageCollider.enabled = false;
    }

    public void Attack()
    {
        _animator.StopMove();
        _animator.Attack();
    }

    public void Move()
    {
        _animator.Move();
        _damageCollider.enabled = false;
    }

    public void StopMove() =>
        _animator.StopMove();

    public void OnDealDamage() =>
        _damageCollider.enabled = true;

    public void OnAttackEnded() => 
        _damageCollider.enabled = false;

    public void OnTakeDame() =>
        HitEffect.Flash();
}
