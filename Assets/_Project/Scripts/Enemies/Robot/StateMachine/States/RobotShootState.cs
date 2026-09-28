class RobotShootState : CharacterState
{
    private readonly IShootingAttackState _shootingAttack;

    public RobotShootState(CharacterStateMachine stateMachine,
        IShootingAttackState shootingAttack,
        IWaitWeaponReload waitWeaponReload)
        : base(stateMachine)
    {
        _shootingAttack = shootingAttack;

        Transitions = new CharacterTransition[]
        {
            new PlayerExitedTransition(stateMachine, shootingAttack),
            new StartReloadWeaponTransition(stateMachine, waitWeaponReload)
        };
    }

    public override StatesType Type => StatesType.Attack;

    public override void Enter() =>
        _shootingAttack.OnEnterShootState();

    public override void Update() =>
        _shootingAttack.ShootingUpdate();

    public override void Exit() =>
        _shootingAttack.OnExitShootState();
}