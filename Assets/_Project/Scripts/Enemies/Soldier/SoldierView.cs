using UnityEngine;

public class SoldierView : EnemyView
{
    [SerializeField] private WeaponView _weaponView;
    [SerializeField] private SoldierBehaviour _behavior;

    public WeaponView Weapon => _weaponView;
    public SoldierBehaviour Behavior => _behavior;

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
