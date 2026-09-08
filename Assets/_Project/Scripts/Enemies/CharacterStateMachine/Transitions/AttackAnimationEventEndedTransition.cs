class AttackAnimationEventEndedTransition : CharacterTransition
{
    private readonly AttackWithAnimationEventState _state;

    public AttackAnimationEventEndedTransition(CharacterStateMachine stateMachine, AttackWithAnimationEventState state)
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
        StateMachine.ChangeState(StatesType.Idle);
    }
}