class IdleState : CharacterState
{
    private readonly IIdleState _idle;

    public IdleState(CharacterStateMachine stateMachine,EnemyModel model,
        IIdleState idle, IMoveState moveState = null) : base(stateMachine)
    {
        _idle = idle;

        Transitions = moveState != null
            ? (new CharacterTransition[]
                {
                    new MoveToTargetTransition(stateMachine, moveState, model),
                })
            : (new CharacterTransition[]
                {
                    new ShootingToPlayerTransition(stateMachine, _idle),
                });
    }

    public override StatesType Type => StatesType.Idle;

    public override void Enter()
    {
        _idle.OnIdleStateEnter();
    }
}
