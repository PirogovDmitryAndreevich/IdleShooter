class StartReloadWeaponTransition : CharacterTransition
{
    private readonly IWaitWeaponReload _weaponReload;

    public StartReloadWeaponTransition(CharacterStateMachine stateMachine,
        IWaitWeaponReload weaponReload) : base(stateMachine)
    {
        _weaponReload = weaponReload;
    }

    public override bool IsNeedTransit()
    {
        return !_weaponReload.IsCanShoot;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Idle);
    }
}