using UnityEngine;

public class SoldierView : EnemyView
{
    [SerializeField] private WeaponView _weaponView;

    public WeaponView Weapon => _weaponView;

    public void SetMoveAnimation(bool isMoving) => 
        _animator.SetBool("IsMoving", isMoving);

    public void SetAttackAnimation(bool isAttack) =>
        _animator.SetBool("IsAttack", isAttack);
}
