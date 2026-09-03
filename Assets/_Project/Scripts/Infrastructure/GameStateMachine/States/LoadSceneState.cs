using System.Collections.Generic;

public class LoadSceneState : IPayloadState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IWeaponDataService _weaponDataService;
    private readonly ILevelController _levelController;
    private Dictionary<LevelType, LevelPresenter> _levels;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        IWeaponDataService weaponDataService, ILevelController levelController)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _weaponDataService = weaponDataService;
        _levelController = levelController;

        _levels = new Dictionary<LevelType, LevelPresenter>();
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
        _levelController.Initialize();

        _weaponDataService.ForInitialize();
    }
}