using System;

public class LoadProgressState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ISaveLoadService _saveLoadService;
    private readonly IProgressService _progressService;

    public LoadProgressState(GameStateMachine gameStateMachine, ISaveLoadService saveLoadService, IProgressService progressService)
    {
        _gameStateMachine = gameStateMachine;
        _saveLoadService = saveLoadService;
        _progressService = progressService;
    }

    public void Enter()
    {
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
