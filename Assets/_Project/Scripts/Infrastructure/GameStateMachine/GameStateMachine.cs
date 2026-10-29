using System;
using System.Collections.Generic;

public class GameStateMachine
{
    private Dictionary<Type, IExitingState> _states;
    private IExitingState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, AllServices allServices, ICoroutineRunner coroutineRunner)
    {
        _states = new Dictionary<Type, IExitingState>
        {
            [typeof(BootstrapState)] = new BootstrapState(this, allServices, sceneLoader, coroutineRunner),

            [typeof(LoadProgressState)] = new LoadProgressState(this,
            allServices.Single<ISaveLoadService>(),
            allServices.Single<IProgressService>(),
            allServices.Single<ILocalizationService>()),

            [typeof(LoadSceneState)] = new LoadSceneState(this,
            sceneLoader,
            allServices.Single<IWeaponDataService>(),
            allServices.Single<ILevelController>(),
            allServices.Single<IProgressService>()),

            [typeof(TutorialState)] = new TutorialState(this,
            coroutineRunner,
            allServices.Single<IProgressService>(),
            allServices.Single<ITutorialService>(),
            allServices.Single<ISaveLoadService>(),
            allServices.Single<ILevelManager>(),
            allServices.Single<ITransitionDataService>(),
            allServices.Single<ILevelController>()),

            [typeof(GameLoopState)] = new GameLoopState(this,
            allServices.Single<ILevelController>(),
            allServices.Single<ITransitionDataService>(),
            allServices.Single<ITimeRewardManager>(),
            allServices.Single<IReviewHandler>(),
            coroutineRunner),
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
