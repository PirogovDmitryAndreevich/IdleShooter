class TakeDamageTransition : CharacterTransition
{
    private readonly ITakingDamage _takingDamage;

    public TakeDamageTransition(CharacterStateMachine stateMachine,
        ITakingDamage takingDamage) : base(stateMachine)
    {
        _takingDamage = takingDamage;
    }

    public override bool IsNeedTransit() =>
        _takingDamage.IsTakedDamage;

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.TakeDamage);
    }
}