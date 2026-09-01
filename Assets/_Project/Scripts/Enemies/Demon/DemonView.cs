using UnityEngine;

public class DemonView : EnemyView
{
    [SerializeField] private AnimationEvent _attackEvent;
    [SerializeField] private MeleeDamageCollider _damageCollider;
    [SerializeField] private DemonBehaviour _demonBehaviour;

    public AnimationEvent AttackEvent =>
        _attackEvent;
    public DemonBehaviour DemonBehaviour =>
        _demonBehaviour;
    public MeleeDamageCollider DamageCollider =>
        _damageCollider;

    public void WaitState()
    {
        _animator.SetBool("IsWalk", false);
        _damageCollider.enabled = false;
    }

    public void Attack()
    {
        _animator.SetBool("IsWalk", false);
        _animator.SetTrigger("Attack");
    }

    public void Move()
    {
        _animator.SetBool("IsWalk", true);
        _damageCollider.enabled = false;
    }

    public void StopMove() =>
        _animator.SetBool("IsWalk", false);

    public void OnDealDamage() =>
        _damageCollider.enabled = true;

    public void OnAttackEnded() =>
        _damageCollider.enabled = false;

    public void OnTakeDame() =>
        HitEffect.Flash();
}
