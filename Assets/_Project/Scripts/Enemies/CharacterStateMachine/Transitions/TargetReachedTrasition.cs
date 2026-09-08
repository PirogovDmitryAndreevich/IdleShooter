class TargetReachedTransition : CharacterTransition
{
    private readonly IMoveState _moveState;
    private readonly EnemyModel _model;

    public TargetReachedTransition(CharacterStateMachine stateMachine, IMoveState moveState, EnemyModel model)
        : base(stateMachine)
    {
        _moveState = moveState;
        _model = model;
    }

    public override bool IsNeedTransit()
    {
        float sqrDistance = (_moveState.Transform.position - _moveState.Target.position).sqrMagnitude;

        return _moveState.IsPlayerInCollider && sqrDistance <= _model.AttackDistance * _model.AttackDistance;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Attack);
    }
}
