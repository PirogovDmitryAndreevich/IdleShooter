class MoveState : CharacterState
{
    private readonly IMoveState _moveState;

    public MoveState(CharacterStateMachine stateMachine, IMoveState moveState, EnemyModel model) : base(stateMachine)
    {
        _moveState = moveState;

        Transitions = new CharacterTransition[]
        {
            new TargetReachedTransition(stateMachine, _moveState, model),
            new WaitPointReachedTransition(stateMachine, _moveState, model)
        };
    }

    public override StatesType Type => StatesType.Move;

    public override void Enter() => 
        _moveState.OnMoveStateEnter();

    public override void Update()
    {
        if (_moveState.MoveTarget == null)
            return;

        _moveState.MovingUpdate();
    }

    public override void Exit() => 
        _moveState.OnMoveStateExit();
}