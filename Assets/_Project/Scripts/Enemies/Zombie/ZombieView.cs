using UnityEngine;

public class ZombieView : EnemyView
{
    [SerializeField] private AnimationEvent _attackEvent;
    [SerializeField] private MeleeDamageCollider _damageCollider;
    [SerializeField] private ZombieBehaviour _zombieBehaviour;

    public AnimationEvent AttackEvent =>
        _attackEvent;
    public ZombieBehaviour ZombieBehaviour =>
        _zombieBehaviour;
    public MeleeDamageCollider DamageCollider =>
        _damageCollider;

    public void Idle()
    {
        _animator.Idle();
        _damageCollider.Enable(false);
    }

    public void Attack()
    {
        _animator.StopMove();
        _animator.Attack();
    }

    public void Move()
    {
        _animator.Move();
        _damageCollider.Enable(false);
    }

    public void StopMove() =>
        _animator.StopMove();

    public void OnDealDamage() =>
        _damageCollider.Enable(true);

    public void OnAttackEnded() =>
        _damageCollider.Enable(false);

    public void OnTakeDame() =>
        HitEffect.Flash();
}
