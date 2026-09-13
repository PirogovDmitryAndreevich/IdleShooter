class MoveToTargetTransition : CharacterTransition
{
    private readonly IMoveState _moveState;
    private readonly EnemyModel _model;

    public MoveToTargetTransition(CharacterStateMachine stateMachine, IMoveState moveState, EnemyModel model)
        : base(stateMachine)
    {
        _moveState = moveState;
        _model = model;
    }

    public override bool IsNeedTransit()
    {
        if(_moveState.MoveTarget == null)
            return false;

        return _moveState.IsPlayerInCollider ||
        IsNeedMoveToWaitPoint();
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Move);
    }

    private bool IsNeedMoveToWaitPoint()
    {
        float sqrDistance = (_moveState.Transform.position - _moveState.MoveTarget.position).sqrMagnitude;

        return !_moveState.IsPlayerInCollider && sqrDistance >= _model.StopMoveDistance * _model.StopMoveDistance;
    }
}
