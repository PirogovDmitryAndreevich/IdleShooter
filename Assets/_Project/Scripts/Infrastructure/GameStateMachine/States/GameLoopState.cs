using System.Collections;
using UnityEngine;

public class GameLoopState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ILevelController _levelController;
    private readonly ITransitionDataService _transitionDataService;
    private readonly ITimeRewardManager _timeRewardManager;
    private readonly ICoroutineRunner _coroutineRunner;

    private PlayerPresenter _player;
    private Coroutine _gameLoopCoroutine;
    private bool _isGameLoopRunning;

    public GameLoopState(GameStateMachine gameStateMachine, ILevelController levelController,
        ITransitionDataService transitionDataService, ITimeRewardManager timeRewardManager,
        ICoroutineRunner coroutineRunner)
    {
        _gameStateMachine = gameStateMachine;
        _levelController = levelController;
        _transitionDataService = transitionDataService;
        _timeRewardManager = timeRewardManager;
        _coroutineRunner = coroutineRunner;
    }
    public void Enter()
    {
        _player = _transitionDataService.Player;

        if (_player == null)
            _transitionDataService.PlayerCreated += OnPlayerCreated;
        else
            SetPlayer();

        _isGameLoopRunning = true;

        _gameLoopCoroutine = _coroutineRunner.StartCoroutine(GameLoopCoroutine());
    }

    public void Exit()
    {
        _levelController.Dispose();
        _player.PlayerDied -= OnPlayerDied;

        _transitionDataService?.Dispose();

        _isGameLoopRunning = false;

        if(_gameLoopCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_gameLoopCoroutine);
            _gameLoopCoroutine = null;
        }
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

    private IEnumerator GameLoopCoroutine()
    {
        while (_isGameLoopRunning)
        {
            _timeRewardManager.Update();

            if (_transitionDataService.CurrentLevel != null && 
                _transitionDataService.CurrentLevel.CheckCompleted())            
                _transitionDataService.OpenNextLevel();            

            yield return null;
        }
    }
}
