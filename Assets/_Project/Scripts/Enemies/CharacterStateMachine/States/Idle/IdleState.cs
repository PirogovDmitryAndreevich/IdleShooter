class IdleState : CharacterState
{
    private readonly IIdleState _idle;

    public IdleState(CharacterStateMachine stateMachine, IIdleState idle) : base(stateMachine)
    {
        _idle = idle;

        Transitions = new CharacterTransition[]
        {
            new WaitPlayerTransition(stateMachine, _idle)
        };
    }

    public override StatesType Type => StatesType.Idle;

    public override void Enter()
    {
        _idle.OnIdleStateEnter();
    }
}
