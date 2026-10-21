public class LoadProgressState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ISaveLoadService _saveLoadService;
    private readonly IProgressService _progressService;
    private readonly ILocalizationService _localizationService;

    public LoadProgressState(GameStateMachine gameStateMachine,
        ISaveLoadService saveLoadService, IProgressService progressService,
        ILocalizationService localizationService)
    {
        _gameStateMachine = gameStateMachine;
        _saveLoadService = saveLoadService;
        _progressService = progressService;
        _localizationService = localizationService;
    }

    public void Enter()
    {
        _localizationService.SetLanguage();
        LoadProgressOrInitNew();
        _gameStateMachine.Enter<LoadSceneState, string>("Main");
    }

    public void Exit()
    {
    }

    private void LoadProgressOrInitNew()
    {
        _progressService.Data =
            _saveLoadService.
            LoadProgress()
            ?? NewProgress();
    }

    private SaveData NewProgress() =>
        new SaveData();
}
