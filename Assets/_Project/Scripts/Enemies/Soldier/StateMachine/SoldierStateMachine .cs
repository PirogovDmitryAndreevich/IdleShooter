using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStateMachine : IDisposable
{
    private enum SoldierState
    {
        Idle,
        MoveToAttack,
        Attack,
        MoveToSpawn
    }

    private readonly SoldierView _view;
    private readonly EnemyShooter _weapon;
    private readonly SoldierModel _model;
    private readonly Transform _spawnPoint;
    private readonly Transform _attackPoint;

    private readonly SoldierMoveState _moveState;

    private readonly Dictionary<SoldierState, ISoldierState> _states;

    private ISoldierState _currentState;
    private SoldierState _currentStateType;

    private Transform _playerTarget;

    public SoldierStateMachine(
        SoldierView view,
        EnemyShooter weapon,
        SoldierModel model,
        Transform spawnPoint,
        Transform attackPoint,
        ICoroutineRunner coroutineRunner)
    {
        _view = view;
        _weapon = weapon;
        _model = model;
        _spawnPoint = spawnPoint;
        _attackPoint = attackPoint;

        RotateToTarget rotateCharacter = new RotateToTarget(model, view);

        _states = new Dictionary<SoldierState, ISoldierState>
        {
            {
                SoldierState.Idle,
                new SoldierIdleState(_view)
            },

            {
                SoldierState.MoveToAttack,
                 new SoldierMoveState(
                    _view,
                    _attackPoint,
                    coroutineRunner,
                    OnReachedAttackPoint)
            },

            {
                SoldierState.Attack,
                new SoldierAttackState(
                    _view,
                    _weapon,
                    _model,
                    rotateCharacter,
                    coroutineRunner,
                    GetPlayerTarget)
            },

            {
                SoldierState.MoveToSpawn,
                new SoldierMoveState(
                    _view,
                    _spawnPoint,
                    coroutineRunner,
                    OnReachedSpawnPoint)
            }
        };

        ChangeState(SoldierState.Idle);
    }

    public void SetTarget(Transform target)
    {
        _playerTarget = target;

        ChangeState(SoldierState.MoveToAttack);
    }

    public void RemoveTarget()
    {
        _playerTarget = null;

        ChangeState(SoldierState.MoveToSpawn);
    }

    public void Reset()
    {
        _playerTarget = null;

        ChangeState(SoldierState.Idle);

        if (_view.NavMeshAgent.isOnNavMesh)
        {
            _view.NavMeshAgent.Warp(
                _spawnPoint.position);
        }
        else
        {
            _view.transform.position =
                _spawnPoint.position;
        }
    }

    public void Dispose()
    {
        _currentState?.Exit();

        _currentState = null;
        _playerTarget = null;
    }

    private Transform GetPlayerTarget()
    {
        return _playerTarget;
    }

    private void ChangeState(SoldierState newStateType)
    {
        if (_currentStateType == newStateType)
            return;

        _currentState?.Exit();

        _currentStateType = newStateType;
        _currentState = _states[newStateType];

        _currentState.Enter();
    }

    private void OnReachedAttackPoint()
    {
        if (_playerTarget == null)
        {
            ChangeState(SoldierState.MoveToSpawn);
            return;
        }

        ChangeState(SoldierState.Attack);
    }

    private void OnReachedSpawnPoint()
    {
        if (_playerTarget != null)
        {
            ChangeState(SoldierState.MoveToAttack);
            return;
        }

        ChangeState(SoldierState.Idle);
    }
}