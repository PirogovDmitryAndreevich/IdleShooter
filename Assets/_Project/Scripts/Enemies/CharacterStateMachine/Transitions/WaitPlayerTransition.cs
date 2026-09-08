class WaitPlayerTransition : CharacterTransition
{
    private readonly IIdleState _idleState;

    public WaitPlayerTransition(CharacterStateMachine stateMachine, IIdleState idleState)
        : base(stateMachine) => 
        _idleState = idleState;

    public override bool IsNeedTransit() =>
        _idleState.IsPlayerInCollider;

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Move);
    }
}
