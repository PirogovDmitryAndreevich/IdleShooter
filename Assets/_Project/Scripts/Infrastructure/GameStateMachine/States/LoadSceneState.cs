public class LoadSceneState : IPayloadState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IWeaponDataService _weaponDataService;
    private readonly ILevelController _levelController;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        IWeaponDataService weaponDataService, ILevelController levelController)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _weaponDataService = weaponDataService;
        _levelController = levelController;
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
        _levelController.SwitchLevel(LevelType.Lobby);

        _weaponDataService.ForInitialize();
    }
}