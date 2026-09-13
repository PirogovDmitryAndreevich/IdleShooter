class PlayerExitedTransition : CharacterTransition
{
    private readonly IShootingAttackState _attackState;

    public PlayerExitedTransition(CharacterStateMachine stateMachine, IShootingAttackState attackState) : base(stateMachine)
    {
        _attackState = attackState;
    }

    public override bool IsNeedTransit() =>
        !_attackState.IsPlayerInCollider;

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Idle);
    }
}