class DemonMoveState : CharacterState
{
    private readonly DemonPresenter _demon;

    public DemonMoveState(CharacterStateMachine stateMachine, DemonPresenter demon, EnemyModel model) : base(stateMachine)
    {
        _demon = demon;

        Transitions = new CharacterTransition[]
        {
            new TargetReachedTransition(stateMachine, _demon, model),
            new WayPointReachedTransition(stateMachine, _demon, model)
        };
    }

    public override void Enter()
    {
        _demon.MoveState();
    }

    public override void Update()
    {
        if(_demon.Target == null)
            return;

        _demon.Moving();
    }

    public override void Exit()
    {
        _demon.StopMove();
    }
}
