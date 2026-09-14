using UnityEngine;
using UnityEngine.AI;

public class ZombieView : EnemyView
{
    [SerializeField] private AnimationEvent _attackEvent;
    [SerializeField] private MeleeDamageCollider _damageCollider;
    [SerializeField] private ZombieBehaviour _zombieBehaviour;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public AnimationEvent AttackEvent =>
        _attackEvent;
    public ZombieBehaviour ZombieBehaviour =>
        _zombieBehaviour;
    public MeleeDamageCollider DamageCollider =>
        _damageCollider;
    public NavMeshAgent NavMeshAgent =>
        _navMeshAgent;

    public void Idle()
    {
        Animator.Idle();
        _damageCollider.Enable(false);
    }

    public void Attack()
    {
        Animator.StopMove();
        Animator.Attack();
    }

    public void Move()
    {
        Animator.Move();
        _damageCollider.Enable(false);
    }

    public void StopMove() =>
        Animator.StopMove();

    public void OnDealDamage() =>
        _damageCollider.Enable(true);

    public void OnAttackEnded() =>
        _damageCollider.Enable(false);

    public void OnTakeDame() =>
        HitEffect.Flash();
}
