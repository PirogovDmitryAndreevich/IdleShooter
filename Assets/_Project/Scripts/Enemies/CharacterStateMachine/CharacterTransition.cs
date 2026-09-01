using System;

abstract class CharacterTransition
{
    protected CharacterStateMachine StateMachine;

    public event Action Transiting;

    protected CharacterTransition(CharacterStateMachine stateMachine) => 
        StateMachine = stateMachine;

    public abstract bool IsNeedTransit();

    public virtual void Transit() => 
        Transiting?.Invoke();
}