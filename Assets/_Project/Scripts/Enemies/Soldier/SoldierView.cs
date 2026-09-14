using UnityEngine;
using UnityEngine.AI;

public class SoldierView : EnemyView
{
    [SerializeField] private WeaponView _weaponView;
    [SerializeField] private SoldierBehaviour _behavior;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public WeaponView Weapon => _weaponView;
    public SoldierBehaviour Behaviour => _behavior;
    public NavMeshAgent NavMeshAgent =>
        _navMeshAgent;

    public void Idle() =>
        Animator.Idle();

    public void Move() =>
        Animator.Move();

    public void StopMove() =>
        Animator.StopMove();

    public void Shooting() => 
        Animator.Shooting();

    public void StopShooting() =>
        Animator.StopShooting();
}
