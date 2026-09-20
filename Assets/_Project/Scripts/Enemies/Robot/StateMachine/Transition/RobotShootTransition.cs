class RobotShootTransition : CharacterTransition
{
    private readonly IShootingAttackState _shootingAttack;
    private readonly IWaitWeaponReload _weaponReload;

    public RobotShootTransition(CharacterStateMachine stateMachine, IShootingAttackState shootingAttack,
        IWaitWeaponReload weaponReload) : base(stateMachine)
    {
        _shootingAttack = shootingAttack;
        _weaponReload = weaponReload;
    }

    public override bool IsNeedTransit()
    {
        return
            _shootingAttack.IsPlayerInCollider
            && _weaponReload.IsCanShoot;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Shoot);
    }
}
