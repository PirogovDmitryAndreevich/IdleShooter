using UnityEngine;

public class EnemiesAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void Idle() => 
        _animator.SetBool("IsWalk", false);

    public void Attack() => 
        _animator.SetTrigger("Attack");

    public void Move() => 
        _animator.SetBool("IsWalk", true);

    public void StopMove() =>
        _animator.SetBool("IsWalk", false);

    public void Shooting() =>
        _animator.SetBool("IsShooting", true);

    public void StopShooting() =>
        _animator.SetBool("IsShooting", false);

    public void SetMoveAnimation(bool isMoving) =>
        _animator.SetBool("IsMoving", isMoving);

    public void SetAttackAnimation(bool isAttack) =>
        _animator.SetBool("IsAttack", isAttack);
}