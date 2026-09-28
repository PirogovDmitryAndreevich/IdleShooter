class RobotIdleState : CharacterState
{
    private readonly IIdleState _idle;

    public RobotIdleState(CharacterStateMachine stateMachine,EnemyModel model,
        IIdleState idle, IMoveState moveState, IWaitWeaponReload waitWeaponReload,
        IShootingAttackState shootingAttack,
        ITakingDamage takingDamage) : base(stateMachine)
    {
        _idle = idle;

        Transitions = new CharacterTransition[]
        {
            new RobotMoveToTargetTransition(stateMachine, moveState, waitWeaponReload, model),
            new RobotShootTransition(stateMachine, shootingAttack, waitWeaponReload),
            new TakeDamageTransition(stateMachine, takingDamage),
            new KickTransition(stateMachine, model, moveState)
        };
    }

    public override StatesType Type => StatesType.Idle;
}
