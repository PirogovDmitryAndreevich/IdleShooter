using System.Collections;
using UnityEngine;

public class TutorialState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly IProgressService _progressService;
    private readonly ITutorialService _tutorialService;
    private readonly ISaveLoadService _save;
    private readonly ILevelManager _levelManager;
    private readonly ITransitionDataService _transitionDataService;
    private readonly ILevelController _levelController;
    private readonly IStaticDataService _staticData;

    private bool _isLevelUpStudiedStarted;
    private bool _isGiveWinsStudied;
    private Coroutine _coroutine;
    private Coroutine _saveCoroutine;
    private PlayerPresenter _player;
    private float _saveTime;
    private float _saveTimer;
    private bool _saveCoroutineStarted;

    public TutorialState(
        GameStateMachine gameStateMachine,
        ICoroutineRunner coroutineRunner,
        IProgressService progressService,
        ITutorialService tutorialService,
        ISaveLoadService saveLoadService,
        ILevelManager levelManager,
        ITransitionDataService transitionDataService,
        ILevelController levelController,
        IStaticDataService staticData)
    {
        _gameStateMachine = gameStateMachine;
        _coroutineRunner = coroutineRunner;
        _progressService = progressService;
        _tutorialService = tutorialService;
        _save = saveLoadService;
        _levelManager = levelManager;
        _transitionDataService = transitionDataService;
        _levelController = levelController;
        _staticData = staticData;
    }

    public void Enter()
    {
        _player = _transitionDataService.Player;

        if (_player == null)
            _transitionDataService.PlayerCreated += OnPlayerCreated;
        else
            SetPlayer();

        if (!_progressService.Data.Tutorial.IsLevelUpStudied)
        {
            _levelManager.LevelChanged += OnLevelUp;
            _isLevelUpStudiedStarted = true;
        }

        _saveTime = _staticData.GetGameData().SaveTime;
        _saveTimer = _saveTime;

        if(_saveCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_saveCoroutine);
            _saveCoroutineStarted = false;
            _saveCoroutine = null;
        }

        _saveCoroutineStarted = true;
        _coroutineRunner.StartCoroutine(SaveCoroutine());

        if (_coroutine != null)
        {
            _coroutineRunner.StopCoroutine(_coroutine);
            _coroutine = null;
        }        

        _coroutine = _coroutineRunner
            .StartCoroutine(TutorialCoroutine());
    }

    public void Exit()
    {
        if (_coroutine != null)
        {
            _coroutineRunner.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if (_saveCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_saveCoroutine);
            _saveCoroutineStarted = false;
            _saveCoroutine = null;
        }

        if (_isLevelUpStudiedStarted)
            _levelManager.LevelChanged -= OnLevelUp;

        _player.PlayerDied -= OnPlayerDied;
    }

    public IEnumerator TutorialCoroutine()
    {
        while (!_tutorialService.IsReady)
            yield return null;

        _tutorialService.Hud.StartTutorial();

        _tutorialService.Player.StartDrawLine(_tutorialService.StonesSwitcher.Transform);

        while (!_tutorialService.StonesSwitcher.IsSelected)
            yield return null;

        _tutorialService.Player.StopDrawLine();
        _save.SaveProgress();

        if (!_progressService.Data.Tutorial.IsAimButtonStudied)
        {
            Hud hud = _tutorialService.Hud;

            hud.TutorialUI.ShowAimTutorialMessage();

            while (!hud.AimButton.IsAim)
                yield return null;

            hud.TutorialUI.HideAimTutorialMessage();

            _progressService.Data.Tutorial.IsAimButtonStudied = true;
            _save.SaveProgress();
        }

        if (!_progressService.Data.Tutorial.IsPassiveShootingStudied)
        {
            Hud hud = _tutorialService.Hud;

            _tutorialService.Player.StartDrawLine(_tutorialService.PassiveShooting.transform);

            while (!_tutorialService.PassiveShooting.IsPlayerInTrigger)
                yield return null;

            _tutorialService.Player.StopDrawLine();

            hud.TutorialUI.ShowScoreMessage();
            _progressService.Data.Tutorial.IsPassiveShootingStudied = true;
            _save.SaveProgress();
        }

        if (!_progressService.Data.Tutorial.IsGameLevelStudied)
        {
            _isGiveWinsStudied = false;
            ObstaclePresenter obstacle = _tutorialService.LevelView_1.Targets[0];

            _tutorialService.Player.StartDrawLine(obstacle.View.Transform);

            while (!obstacle.IsDestroyed)
                yield return null;

            if (_transitionDataService.CurrentLevel != null)
                _transitionDataService.OpenNextLevel();

            obstacle = _tutorialService.LevelView_1.Targets[1];
            _tutorialService.Player.StartDrawLine(obstacle.View.Transform);

            while (!obstacle.IsDestroyed)
                yield return null;

            _tutorialService.Hud.TutorialUI.ShowCrownMessage();

            obstacle = _tutorialService.LevelView_1.Targets[2];
            _tutorialService.Player.StartDrawLine(obstacle.View.Transform);

            while (!obstacle.IsDestroyed)
                yield return null;

            obstacle = _tutorialService.LevelView_1.Targets[3];
            _tutorialService.Player.StartDrawLine(obstacle.View.Transform);

            while (!obstacle.IsDestroyed)
                yield return null;

            obstacle = _tutorialService.LevelView_1.Targets[4];
            _tutorialService.Player.StartDrawLine(obstacle.View.Transform);

            while (!obstacle.IsDestroyed)
                yield return null;

            _tutorialService.Transition2.PlayerGiveWins += OnPlayerGiveWins;
            _tutorialService.Player.StartDrawLine(_tutorialService.Transition2.WinsPoint);

            while (!_isGiveWinsStudied)
                yield return null;

            _tutorialService.Player.StopDrawLine();

            yield return new WaitForSeconds(1f);

            _tutorialService.Hud.TutorialUI.ShowGiveWinsMessage();

            _tutorialService.Hud.TutorialEnded();
            _progressService.Data.Tutorial.IsGameLevelStudied = true;
            _save.SaveProgress();
        }


        _progressService.Data.Tutorial.IsTutorialComplete = true;
        _save.SaveProgress();

        _coroutine = null;

        _gameStateMachine.Enter<GameLoopState>();
    }

    public IEnumerator SaveCoroutine()
    {
        while (_saveCoroutineStarted)
        {
            _saveTimer -= Time.deltaTime;
            if (_saveTimer <= 0)
            {
                _save.SaveProgress();
                _saveTimer = _saveTime;
            }
            yield return null;
        }

        _saveCoroutine = null;
        _saveCoroutineStarted = false;
    }

    private void OnPlayerGiveWins()
    {
        _tutorialService.Transition2.PlayerGiveWins -= OnPlayerGiveWins;
        _isGiveWinsStudied = true;

    }

    private void OnLevelUp(int obj)
    {
        _levelManager.LevelChanged -= OnLevelUp;
        _isLevelUpStudiedStarted = false;

        _tutorialService.Hud.TutorialUI.ShowLevelUpMessage();
        _progressService.Data.Tutorial.IsLevelUpStudied = true;
        _save.SaveProgress();
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

    private void OnPlayerDied()
    {
        _levelController.WarpToLevel(LevelType.Lobby);
        _transitionDataService.ResetEnteredLevels();
    }
}