class RobotMoveToTargetTransition : CharacterTransition
{
    private readonly IMoveState _moveState;
    private readonly IWaitWeaponReload _waitReload;
    private readonly EnemyModel _model;

    public RobotMoveToTargetTransition(CharacterStateMachine stateMachine,
        IMoveState moveState,
        IWaitWeaponReload waitReload,
        EnemyModel model) : base(stateMachine)
    {
        _moveState = moveState;
        _waitReload = waitReload;
        _model = model;
    }

    public override bool IsNeedTransit()
    {
        if (_moveState.MoveTarget == null)
            return false;

        return IsNeedMoveToPlayer() ||
        IsNeedMoveToWaitPoint();
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Move);
    }

    private bool IsNeedMoveToPlayer()
    {
        float sqrDistance = (_moveState.Transform.position - _moveState.MoveTarget.position).sqrMagnitude;

        return
            _moveState.IsPlayerInCollider
            && !_waitReload.IsCanShoot
            && sqrDistance >= _model.AttackDistance * _model.AttackDistance;
    }

    private bool IsNeedMoveToWaitPoint()
    {
        float sqrDistance = (_moveState.Transform.position - _moveState.MoveTarget.position).sqrMagnitude;

        return
            !_moveState.IsPlayerInCollider
            && sqrDistance >= _model.StopMoveDistance * _model.StopMoveDistance;
    }
}
