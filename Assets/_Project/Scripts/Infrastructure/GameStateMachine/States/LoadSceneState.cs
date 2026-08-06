using UnityEngine;

public class LoadSceneState : IPayloadState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IGameFactory _gameFactory;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _gameFactory = gameFactory;
    }

    public void Enter(string payload)
    {
        _sceneLoader.Load(payload, OnLoaded);
    }

    public void Exit()
    {
        
    }

    private void OnLoaded()
    {
        var atPlayer = GameObject.FindWithTag("PlayerSpawnPoint").transform.position;
        _gameFactory.CreatePlayer(atPlayer);
    }
}