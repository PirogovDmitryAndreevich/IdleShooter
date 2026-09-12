using UnityEngine;

public class SoldierView : EnemyView
{
    [SerializeField] private WeaponView _weaponView;

    public WeaponView Weapon => _weaponView;

    public void SetMoveAnimation(bool isMoving) => 
        _animator.SetMoveAnimation(isMoving);

    public void SetAttackAnimation(bool isAttack) =>
        _animator.SetMoveAnimation(isAttack);
}
