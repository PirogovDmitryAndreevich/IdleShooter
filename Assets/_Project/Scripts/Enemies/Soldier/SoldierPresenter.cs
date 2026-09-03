using UnityEngine;

public class SoldierPresenter : EnemyPresenter
{
    private readonly PlayerTriggerObserver _triggerObserver;
    private readonly SoldierStateMachine _stateMachine;

    public SoldierPresenter(EnemyModel model, SoldierView view,
        PlayerTriggerObserver triggerObserver, SoldierStateMachine stateMachine)
        : base(view, model)
    {
        _triggerObserver = triggerObserver;

        _stateMachine = stateMachine;

        _triggerObserver.TargetEnter += SetTarget;
        _triggerObserver.PlayerExited += RemoveTarget;
    }

    public override void Dispose()
    {
        base.Dispose();

        _triggerObserver.TargetEnter -= SetTarget;
        _triggerObserver.PlayerExited -= RemoveTarget;

        _stateMachine.Dispose();
    }

    public override void ResetObject()
    {
        base.ResetObject();

        _stateMachine.Reset();
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
