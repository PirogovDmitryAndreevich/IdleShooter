using UnityEngine;
using UnityEngine.AI;

public class RobotView : EnemyView
{
    [SerializeField] private AnimationEvent _animationEvent;
    [SerializeField] private EnemyStepAnimationEvent _stepEvent;
    [SerializeField] private WeaponView _weaponView;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private MeleeDamageCollider _damageCollider;
    [SerializeField] private RobotBehaviour _behaviour;
    [SerializeField] private RobotSounds _sounds;

    public RobotBehaviour Behaviour =>
        _behaviour;
    public EnemyStepAnimationEvent StepEvent =>
        _stepEvent;
    public AnimationEvent AnimationEvent =>
        _animationEvent;
    public NavMeshAgent NavMeshAgent =>
        _navMeshAgent;
    public WeaponView Weapon => _weaponView;
    public MeleeDamageCollider DamageCollider =>
        _damageCollider;
    public RobotSounds Sounds => _sounds;

    public void Idle()
    {
        Animator.Idle();
        _damageCollider.enabled = false;
    }

    public void Kick()
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
    public void Shooting() =>
        Animator.Shooting();

    public void StopShooting() =>
        Animator.StopShooting();

    public void OnTakeDame() =>
        HitEffect.Flash();
}
