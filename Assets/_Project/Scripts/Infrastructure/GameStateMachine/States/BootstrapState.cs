using System;

public class BootstrapState : IState
{
    private const string InitialSceneName = "Initial";

    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _allServices;
    private readonly ICoroutineRunner _coroutineRunner;

    public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices allServices,
        ICoroutineRunner coroutineRunner)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _allServices = allServices;
        _coroutineRunner = coroutineRunner;

        RegisterService();
    }

    public void Enter() =>
        _sceneLoader.Load(InitialSceneName, onLoaded: EnterLoadLevel);

    public void Exit() { }

    private void EnterLoadLevel()
    {
        //_gameStateMachine.Enter<>();
    }

    private void RegisterService()
    {
        
    }
}
