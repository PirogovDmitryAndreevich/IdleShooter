class AttackEndedTransition : CharacterTransition
{
    private readonly DemonAttackState _state;

    public AttackEndedTransition(CharacterStateMachine stateMachine, DemonAttackState state)
        : base(stateMachine)
    {        
        _state = state;
    }

    public override bool IsNeedTransit()
    {
        return _state.IsAttackEnded;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<DemonIdleState>();
    }
}
