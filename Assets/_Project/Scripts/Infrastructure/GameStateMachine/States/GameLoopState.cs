public class GameLoopState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ILevelController _levelController;
    private readonly ITransitionDataService _transitionDataService;
    private readonly ICoroutineRunner _coroutineRunner;

    private PlayerPresenter _player;

    public GameLoopState(GameStateMachine gameStateMachine, ILevelController levelController,
        ITransitionDataService transitionDataService, ICoroutineRunner coroutineRunner)
    {
        _gameStateMachine = gameStateMachine;
        _levelController = levelController;
        _transitionDataService = transitionDataService;
        _coroutineRunner = coroutineRunner;
    }
    public void Enter()
    {
        _player = _transitionDataService.Player;

        if (_player == null)
            _transitionDataService.PlayerCreated += OnPlayerCreated;
        else
            SetPlayer();
    }

    public void Exit()
    {
        _levelController.Dispose();
        _player.PlayerDied -= OnPlayerDied;

        _transitionDataService?.Dispose();
    }

    private void OnPlayerDied()
    {
        _levelController.WarpToLevel(LevelType.Lobby);
        _transitionDataService.ResetEnteredLevels();
    }

    private void OnPlayerCreated()
    {
        _transitionDataService.PlayerCreated -= OnPlayerCreated;

        SetPlayer();
    }

    private void SetPlayer()
    {
        _player = _transitionDataService.Player;
        _player.PlayerDied += OnPlayerDied;
    }
}
