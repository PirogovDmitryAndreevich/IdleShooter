using UnityEngine;
using UnityEngine.AI;

public class DemonView : EnemyView
{
    [SerializeField] private AnimationEvent _attackEvent;
    [SerializeField] private MeleeDamageCollider _damageCollider;
    [SerializeField] private DemonBehaviour _demonBehaviour;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private DemonSounds _sounds;

    public AnimationEvent AttackEvent =>
        _attackEvent;
    public DemonBehaviour DemonBehaviour =>
        _demonBehaviour;
    public MeleeDamageCollider DamageCollider =>
        _damageCollider;
    public NavMeshAgent NavMeshAgent =>
        _navMeshAgent;
    public DemonSounds Sounds => _sounds;

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
}
