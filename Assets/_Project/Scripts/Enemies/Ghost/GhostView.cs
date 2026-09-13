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
        Animator.Idle();
        _damageCollider.enabled = false;
    }

    public void Attack()
    {
        Animator.StopMove();
        Animator.Attack();
    }

    public void Move()
    {
        Animator.Move();
        _damageCollider.enabled = false;
    }

    public void StopMove() =>
        Animator.StopMove();

    public void OnDealDamage() =>
        _damageCollider.enabled = true;

    public void OnAttackEnded() => 
        _damageCollider.enabled = false;

    public void OnTakeDame() =>
        HitEffect.Flash();
}
