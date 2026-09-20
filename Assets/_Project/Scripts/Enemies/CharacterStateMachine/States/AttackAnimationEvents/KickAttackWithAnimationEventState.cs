class KickAttackWithAnimationEventState : AttackWithAnimationEventState
{
    public KickAttackWithAnimationEventState(CharacterStateMachine stateMachine, IAttackWithAnimationEvent animationEventAttack, AnimationEvent animationEvent) : base(stateMachine, animationEventAttack, animationEvent)
    {
    }

    public override StatesType Type => StatesType.Kick;
}