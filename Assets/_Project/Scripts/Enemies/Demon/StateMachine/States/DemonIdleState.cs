class DemonIdleState : CharacterState
{
    private readonly DemonPresenter _demon;

    public DemonIdleState(CharacterStateMachine stateMachine, DemonPresenter demon) : base(stateMachine)
    {
        _demon = demon;

        Transitions = new CharacterTransition[]
        {
            new WaitPlayerTransition(stateMachine, demon)
        };
    }

    public override void Enter() => 
        _demon.WaitState();
}
