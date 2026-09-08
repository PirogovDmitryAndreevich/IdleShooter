using System.Collections.Generic;

abstract class CharacterStateMachine
{
    protected CharacterState CurrentState;
    protected Dictionary<StatesType, CharacterState> States;

    public void Update()
    {
        if (CurrentState == null)
            return;

        CurrentState.Update();

        CurrentState.TryTransit();
    }

    public void ChangeState(StatesType type)
    {
        if (CurrentState != null && CurrentState.Type == type)
            return;

        if (States.TryGetValue(type, out CharacterState newState))
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
