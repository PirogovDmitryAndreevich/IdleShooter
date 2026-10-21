public class LoadSceneState : IPayloadState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IWeaponDataService _weaponDataService;
    private readonly ILevelController _levelController;
    private readonly IProgressService _progressService;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        IWeaponDataService weaponDataService, ILevelController levelController,
        IProgressService progressService)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _weaponDataService = weaponDataService;
        _levelController = levelController;
        _progressService = progressService;
    }

    public void Enter(string payload)
    {
        _sceneLoader.Load(payload, OnLoaded);

        if (_progressService.Data.Tutorial.IsTutorialComplete)
            _gameStateMachine.Enter<GameLoopState>();
        else
            _gameStateMachine.Enter<TutorialState>();
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