class KickTransition : CharacterTransition
{
    private readonly EnemyModel _model;
    private readonly IMoveState _moveState;

    public KickTransition(CharacterStateMachine stateMachine, EnemyModel model, IMoveState moveState) : base(stateMachine)
    {
        _model = model;
        _moveState = moveState;
    }

    public override bool IsNeedTransit()
    {
        float sqrDistance = (_moveState.Transform.position - _moveState.MoveTarget.position).sqrMagnitude;

        return
            _moveState.IsPlayerInCollider
            && sqrDistance <= _model.AttackDistance * _model.AttackDistance;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Kick);
    }
}
