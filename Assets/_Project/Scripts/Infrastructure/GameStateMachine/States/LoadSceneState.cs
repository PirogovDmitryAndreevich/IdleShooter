using UnityEngine;

public class LoadSceneState : IPayloadState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IGameFactory _gameFactory;
    private readonly IWeaponDataService _weaponDataService;
    private readonly IStaticDataService _staticData;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        IGameFactory gameFactory, IWeaponDataService weaponDataService, IStaticDataService staticData)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _gameFactory = gameFactory;
        _weaponDataService = weaponDataService;
        _staticData = staticData;
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
        GameStaticData gameData = _staticData.GetGameData();

        var atPlayer = GameObject.FindWithTag("PlayerSpawnPoint").transform.position;
        _gameFactory.CreatePlayer(atPlayer);

        _weaponDataService.ForInitialize();

        foreach (var switcher in gameData.WeaponSwitcherData)
            _gameFactory.CreateWeaponSwitcher(switcher.WeaponType, switcher.Position);
    }
}