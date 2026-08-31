using System;
using UnityEngine;

public class DemonView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationEvent _attackEvent;
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;

    public AnimationEvent AttackEvent =>
        _attackEvent;

    public void WaitState()
    {
        _animator.SetBool("isWak", false);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
        _playerTriggerObserver.enabled = true;
    }

    public void Move()
    {
        _animator.SetBool("isWak", true);
    }

    public void OnAttackEnded()
    {
        _playerTriggerObserver.enabled = false;
    }
}

public class DemonPresenter
{
    private readonly EnemyModel _model;
    private readonly DemonView _view;

    private bool _isAttack;
    private float _cooldownAttack;

    public DemonPresenter(EnemyModel model, DemonView view)
    {
        _model = model;
        _view = view;

        WaitState();
    }

    private void WaitState()
    {
        _view.WaitState();
    }

    private void MoveState()
    {
        _view.Move();
    }

    private void AttackState()
    {
        
    }

    private void StartAttack()
    {
        if (_isAttack)
            return;

        _isAttack = true;
        _view.Attack();
    }

    private void OnEndedAttackEvent() =>
        EndedAttack();

    private void EndedAttack()
    {
        _isAttack = false;
        _cooldownAttack = Time.time + _model.FireRate;
    }
}