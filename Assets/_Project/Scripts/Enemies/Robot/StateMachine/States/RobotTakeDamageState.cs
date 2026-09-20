class RobotTakeDamageState : CharacterState
{
    private readonly ITakingDamage _takingDamage;

    public RobotTakeDamageState(CharacterStateMachine stateMachine, ITakingDamage takingDamage)
        : base(stateMachine)
    {
        _takingDamage = takingDamage;

        Transitions = new CharacterTransition[]
        {
            new TakeDamageCompleteTransition(stateMachine, takingDamage)
        };
    }

    public override StatesType Type => StatesType.TakeDamage;

    public override void Enter() =>
        _takingDamage.OnTakeDamageStateEnter();

    public override void Update() => 
        _takingDamage.OnTakeDamageStateUpdate();

    public override void Exit() =>
        _takingDamage.OnTakeDamageStateExit();
}
