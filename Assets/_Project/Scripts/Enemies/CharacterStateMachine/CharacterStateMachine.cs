using System;
using System.Collections.Generic;

abstract class CharacterStateMachine
{
    protected CharacterState CurrentState;
    protected Dictionary<Type, CharacterState> States;

    public void Update()
    {
        if (CurrentState == null)
            return;

        CurrentState.Update();

        CurrentState.TryTransit();
    }

    public void ChangeState<TState>() where TState : CharacterState
    {
        if (CurrentState != null && CurrentState.GetType() == typeof(TState))
            return;

        if (States.TryGetValue(typeof(TState), out CharacterState newState))
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
