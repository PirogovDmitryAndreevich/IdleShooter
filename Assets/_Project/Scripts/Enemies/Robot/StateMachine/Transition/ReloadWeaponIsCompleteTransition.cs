class ReloadWeaponIsCompleteTransition : CharacterTransition
{
    private readonly IWaitWeaponReload _weaponReload;
    private readonly IMoveState _moveState;

    public ReloadWeaponIsCompleteTransition(CharacterStateMachine stateMachine,
        IWaitWeaponReload weaponReload, IMoveState moveState) : base(stateMachine)
    {
        _weaponReload = weaponReload;
        _moveState = moveState;
    }

    public override bool IsNeedTransit()
    {
        return _moveState.IsPlayerInCollider
            && _weaponReload.IsCanShoot;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Shoot);
    }
}