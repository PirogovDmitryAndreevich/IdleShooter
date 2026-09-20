class RobotMoveState : CharacterState
{
    private readonly IMoveState _moveState;

    public RobotMoveState(CharacterStateMachine stateMachine,
        IMoveState moveState, EnemyModel model,
        IWaitWeaponReload weaponReload,
        ITakingDamage takingDamage) : base(stateMachine)
    {
        _moveState = moveState;

        Transitions = new CharacterTransition[]
        {
            new KickTransition(stateMachine, model, moveState),
            new ReloadWeaponIsCompleteTransition(stateMachine, weaponReload, moveState),
            new WaitPointReachedTransition(stateMachine, moveState, model),
            new TakeDamageTransition(stateMachine, takingDamage)
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
