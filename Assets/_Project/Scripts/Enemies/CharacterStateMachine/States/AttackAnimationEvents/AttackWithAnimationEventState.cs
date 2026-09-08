class AttackWithAnimationEventState : CharacterState
{
    private readonly IAttackWithAnimationEvent _animationEventAttack;
    private readonly AnimationEvent _animationEvent;
    public bool IsAttackEnded;

    public override StatesType Type => StatesType.Attack;

    public AttackWithAnimationEventState(CharacterStateMachine stateMachine,
        IAttackWithAnimationEvent animationEventAttack,
        AnimationEvent animationEvent)
        : base(stateMachine)
    {
        _animationEventAttack = animationEventAttack;
        _animationEvent = animationEvent;

        Transitions = new CharacterTransition[]
        {
            new AttackAnimationEventEndedTransition(stateMachine, this)
        };
    }

    public override void Enter()
    {
        _animationEvent.AttackEnded += OnAttackEnded;
        _animationEvent.DealDamage += OnDealDamage;
        IsAttackEnded = false;
        _animationEventAttack.OnAttackStateEnter();
    }

    public override void Exit()
    {
        _animationEvent.AttackEnded -= OnAttackEnded;
        _animationEvent.DealDamage -= OnDealDamage;
    }

    private void OnDealDamage()
    {
        _animationEventAttack.OnDealDamage();
    }

    private void OnAttackEnded()
    {
        IsAttackEnded = true;
        _animationEventAttack.OnAttackEnded();
    }
}
