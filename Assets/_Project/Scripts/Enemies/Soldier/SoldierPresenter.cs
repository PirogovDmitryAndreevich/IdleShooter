using System;
using UnityEngine;

public class SoldierPresenter : IDisposable, IResetObject
{
    private readonly SoldierView _view;
    private readonly PlayerTriggerObserver _triggerObserver;
    private readonly SoldierStateMachine _stateMachine;

    public bool IsNeedReset { get; private set; }

    public SoldierPresenter(SoldierModel model, SoldierView view,
        PlayerTriggerObserver triggerObserver, SoldierStateMachine stateMachine)
    {
        _view = view;
        _triggerObserver = triggerObserver;

        _stateMachine = stateMachine;

        _view.DamageableCollider.DamageReceived += TakeDamage;

        _triggerObserver.TargetEnter += SetTarget;
        _triggerObserver.PlayerExited += RemoveTarget;
    }

    public void Dispose()
    {
        _view.DamageableCollider.DamageReceived -= TakeDamage;

        _triggerObserver.TargetEnter -= SetTarget;
        _triggerObserver.PlayerExited -= RemoveTarget;

        _stateMachine.Dispose();
    }

    public void ResetObject()
    {
        _stateMachine.Reset();
    }

    private void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _view.HitEffect.Flash();
    }

    private void SetTarget(Transform target)
    {
        _stateMachine.SetTarget(target);
    }

    private void RemoveTarget()
    {
        _stateMachine.RemoveTarget();
    }
}
