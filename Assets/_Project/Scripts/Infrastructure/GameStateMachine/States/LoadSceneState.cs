using UnityEngine;

public class LoadSceneState : IPayloadState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IGameFactory _gameFactory;
    private readonly IWeaponDataService _weaponDataService;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        IGameFactory gameFactory, IWeaponDataService weaponDataService)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _gameFactory = gameFactory;
        _weaponDataService = weaponDataService;
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

        _weaponDataService.ForInitialize();
    }
}