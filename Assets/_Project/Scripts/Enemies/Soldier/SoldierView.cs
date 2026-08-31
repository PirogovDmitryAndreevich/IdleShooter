using System;
using UnityEngine;
using UnityEngine.AI;

public class SoldierView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private WeaponView _weaponView;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private HitFlash _hitEffect;
    [SerializeField] private DamageableCollider _damageableCollider;

    public HitFlash HitEffect => _hitEffect;
    public NavMeshAgent NavMeshAgent =>
        _navMeshAgent;
    public WeaponView Weapon => _weaponView;
    public DamageableCollider DamageableCollider => 
        _damageableCollider;

    public void SetMoveAnimation(bool isMoving) => 
        _animator.SetBool("IsMoving", isMoving);

    public void SetAttackAnimation(bool isAttack) =>
        _animator.SetBool("IsAttack", isAttack);
}
