using System.Collections;
using UnityEngine;

public class GameLoopState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ILevelController _levelController;
    private readonly ITransitionDataService _transitionDataService;
    private readonly ITimeRewardManager _timeRewardManager;
    private readonly IReviewHandler _reviewHandler;
    private readonly IPlayerDiedService _playerDiedService;
    private readonly ICoroutineRunner _coroutineRunner;
    
    private Coroutine _gameLoopCoroutine;
    private bool _isGameLoopRunning;

    public GameLoopState(GameStateMachine gameStateMachine,
        ILevelController levelController,
        ITransitionDataService transitionDataService, ITimeRewardManager timeRewardManager,
        IReviewHandler reviewHandler, IPlayerDiedService playerDiedService, ICoroutineRunner coroutineRunner)
    {
        _gameStateMachine = gameStateMachine;
        _levelController = levelController;
        _transitionDataService = transitionDataService;
        _timeRewardManager = timeRewardManager;
        _reviewHandler = reviewHandler;
        _playerDiedService = playerDiedService;
        _coroutineRunner = coroutineRunner;
    }
    public void Enter()
    {
        _playerDiedService.PlayerDied += OnPlayerDied;

        _isGameLoopRunning = true;

        _gameLoopCoroutine = _coroutineRunner.StartCoroutine(GameLoopCoroutine());
    }

    public void Exit()
    {
        _transitionDataService?.Dispose();
        _levelController.Dispose();

        _playerDiedService.PlayerDied -= OnPlayerDied;
        _playerDiedService.Dispose();

        _isGameLoopRunning = false;

        if(_gameLoopCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_gameLoopCoroutine);
            _gameLoopCoroutine = null;
        }
    }

    private IEnumerator GameLoopCoroutine()
    {
        _reviewHandler.UpdateData();

        while (_isGameLoopRunning)
        {
            _timeRewardManager.Update();            

            if (_transitionDataService.CurrentLevel != null && 
                _transitionDataService.CurrentLevel.CheckCompleted())            
                _transitionDataService.OpenNextLevel();            

            yield return null;
        }
    }

    private void OnPlayerDied(LevelType level)
    {
        _levelController.WarpToLevel(level);
        _transitionDataService.ResetEnteredLevels();
    }
}
