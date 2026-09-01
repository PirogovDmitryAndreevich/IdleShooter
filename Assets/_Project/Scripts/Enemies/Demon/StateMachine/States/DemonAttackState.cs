class DemonAttackState : CharacterState
{
    private readonly DemonPresenter _demon;
    private readonly AnimationEvent _animationEvent;
    public bool IsAttackEnded;

    public DemonAttackState(CharacterStateMachine stateMachine, DemonPresenter demon, AnimationEvent animationEvent)
        : base(stateMachine)
    {
        _demon = demon;
        _animationEvent = animationEvent;

        Transitions = new CharacterTransition[]
        {
            new AttackEndedTransition(stateMachine, this)
        };        
    }

    public override void Enter()
    {
        _animationEvent.AttackEnded += OnAttackEnded;
        _animationEvent.DealDamage += OnDealDamage;
        IsAttackEnded = false;
        _demon.AttackState();
    }

    public override void Exit()
    {
        _animationEvent.AttackEnded -= OnAttackEnded;
        _animationEvent.DealDamage -= OnDealDamage;
    }

    private void OnDealDamage()
    {
        _demon.OnDealDamage();
    }

    private void OnAttackEnded()
    {
        IsAttackEnded = true;    
        _demon.OnAttackEnded();
    }
}
