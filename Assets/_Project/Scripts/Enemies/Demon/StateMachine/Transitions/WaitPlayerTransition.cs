class WaitPlayerTransition : CharacterTransition
{
    private readonly DemonPresenter _demon;

    public WaitPlayerTransition(CharacterStateMachine stateMachine, DemonPresenter demon)
        : base(stateMachine) => 
        _demon = demon;

    public override bool IsNeedTransit() =>
        _demon.IsPlayerInCollider;

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<DemonMoveState>();
    }
}
