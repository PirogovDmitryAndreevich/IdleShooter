class TakeDamageCompleteTransition : CharacterTransition
{
    private readonly ITakingDamage _takingDamage;

    public TakeDamageCompleteTransition(CharacterStateMachine stateMachine, 
        ITakingDamage takingDamage) : base(stateMachine)
    {
        _takingDamage = takingDamage;
    }

    public override bool IsNeedTransit()
    {
        return _takingDamage.IsTakingDamageCompleted;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState(StatesType.Idle);
    }
}