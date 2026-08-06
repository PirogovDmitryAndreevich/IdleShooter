using System;
using System.Collections.Generic;

public class GameStateMachine
{
    private Dictionary<Type, IExitingState> _states;
    private IExitingState _activeState;

    public GameStateMachine(SceneLoader sceneLoader,ICoroutineRunner coroutineRunner)
    {
        _states = new Dictionary<Type, IExitingState>
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, coroutineRunner),
            [typeof(LoadProgressState)] = new LoadProgressState(this,
            AllServices.Single<ISaveLoadService>(),
            AllServices.Single<IProgressService>()),
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        IState state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
    {
        TState state = ChangeState<TState>();
        state.Enter(payload);
    }

    private TState ChangeState<TState>() where TState : class, IExitingState
    {
        _activeState?.Exit();
        TState state = GetState<TState>();
        _activeState = state;
        return state;
    }

    private TState GetState<TState>() where TState : class, IExitingState =>
        _states[typeof(TState)] as TState;
}
