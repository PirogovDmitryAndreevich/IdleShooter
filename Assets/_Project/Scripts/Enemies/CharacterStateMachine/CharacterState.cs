abstract class CharacterState
{
    protected CharacterTransition[] Transitions;

    protected CharacterState(CharacterStateMachine stateMachine) { }

    public abstract StatesType Type { get; }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void TryTransit()
    {
        foreach (CharacterTransition transition in Transitions)
        {
            if (transition.IsNeedTransit())
            {
                transition.Transit();
                return;
            }
        }
    }
}
