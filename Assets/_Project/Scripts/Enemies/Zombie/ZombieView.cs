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
        _animator.SetBool("IsWalk", false);
        _damageCollider.Enable(false);
    }

    public void Attack()
    {
        _animator.SetBool("IsWalk", false);
        _animator.SetTrigger("Attack");
    }

    public void Move()
    {
        _animator.SetBool("IsWalk", true);
        _damageCollider.Enable(false);
    }

    public void StopMove() =>
        _animator.SetBool("IsWalk", false);

    public void OnDealDamage() =>
        _damageCollider.Enable(true);

    public void OnAttackEnded() =>
        _damageCollider.Enable(false);

    public void OnTakeDame() =>
        HitEffect.Flash();
}
