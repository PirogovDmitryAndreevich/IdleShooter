using System;
using System.Collections;
using UnityEngine;

public class SoldierAttackState : ISoldierState
{
    private readonly SoldierView _view;
    private readonly EnemyShooter _weapon;
    private readonly SoldierModel _model;
    private readonly RotateToTarget _rotateCharacter;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly Func<Transform> _getTarget;

    private Coroutine _attackStateCoroutine;
    private float _nextFireTime = 0f;

    public SoldierAttackState(
        SoldierView view,
        EnemyShooter weapon,
        SoldierModel model,
        RotateToTarget rotateCharacter,
        ICoroutineRunner coroutineRunner,
        Func<Transform> getTarget)
    {
        _view = view;
        _weapon = weapon;
        _model = model;
        _rotateCharacter = rotateCharacter;
        _coroutineRunner = coroutineRunner;
        _getTarget = getTarget;
    }

    public void Enter()
    {
        if (_view.NavMeshAgent.isOnNavMesh)
            _view.NavMeshAgent.ResetPath();

        _view.SetMoveAnimation(false);
        _view.SetAttackAnimation(true);

        Transform target = _getTarget();

        StopCoroutine();

        _attackStateCoroutine =
            _coroutineRunner.
            StartCoroutine(AttackStateCoroutine(target));
    }

    public void Exit()
    {
        _view.SetAttackAnimation(false);

        StopCoroutine();
    }

    private IEnumerator AttackStateCoroutine(Transform target)
    {
        while (true)
        {
            if(target != null)
                yield return null;

            if (TrySeeTarget(target))            
                TryShoot(target);

            _rotateCharacter.UpdateRotation(target);

            yield return null;
        }
    }

    private void TryShoot(Transform target)
    {
        if (Time.time < _nextFireTime)
            return;

        _nextFireTime = Time.time + _model.FireRate;

        _weapon.Shoot(target);
    }

    private bool TrySeeTarget(Transform target)
    {
        const float EyeHeight = 0.7f;

        Vector3 origin = _view.transform.position + Vector3.up * EyeHeight;
        Vector3 targetPosition = target.position + Vector3.up * EyeHeight;

        Vector3 direction = targetPosition - origin;
        float distance = direction.magnitude;

        if (distance > _model.AttackDistance)
        {
            Debug.DrawLine(origin, targetPosition, Color.white);

            return false;
        }

        if (!Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance))
        {
            Debug.DrawLine(origin, targetPosition, Color.white);
            return false;
        }

        bool seesTarget =
            hit.transform == target ||
            hit.transform.IsChildOf(target);

        Debug.DrawLine(
            origin,
            hit.point,
            seesTarget ? Color.red : Color.white);

        return seesTarget;
    }

    private void StopCoroutine()
    {
        if (_attackStateCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_attackStateCoroutine);
            _attackStateCoroutine = null;
        }
    }
}