using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SoldierMoveState : ISoldierState
{
    private readonly SoldierView _view;
    private readonly Transform _target;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly AgentMoveToTarget _agentMoveToTarget;
    private readonly Action _onReached;

    private Coroutine _moveCoroutine;

    public SoldierMoveState(
        SoldierView view,
        Transform target,
        ICoroutineRunner coroutineRunner,
        Action onReached)
    {
        _view = view;
        _target = target;
        _coroutineRunner = coroutineRunner;
        _onReached = onReached;

        _agentMoveToTarget = new AgentMoveToTarget(
            _view.NavMeshAgent);
    }

    public void Enter()
    {
        _view.SetAttackAnimation(false);
        _view.SetMoveAnimation(true);

        _moveCoroutine = _coroutineRunner.StartCoroutine(Move());
    }

    public void Exit()
    {
        StopMovement();

        _view.SetMoveAnimation(false);
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if (_target == null)
                yield break;

            _agentMoveToTarget.Move(_target);

            if (HasReachedTarget())
            {
                _moveCoroutine = null;

                _onReached?.Invoke();

                yield break;
            }

            yield return null;
        }
    }

    private bool HasReachedTarget()
    {
        NavMeshAgent agent = _view.NavMeshAgent;

        if (!agent.isOnNavMesh)
            return false;

        if (agent.pathPending)
            return false;

        return agent.remainingDistance <= agent.stoppingDistance;
    }

    private void StopMovement()
    {
        if (_moveCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        if (_view.NavMeshAgent.isOnNavMesh)
            _view.NavMeshAgent.ResetPath();
    }
}
