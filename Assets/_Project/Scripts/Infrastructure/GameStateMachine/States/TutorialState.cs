using System.Collections;
using UnityEditor.MPE;
using UnityEngine;

public class TutorialState : IState
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly IProgressService _progressService;
    private readonly ITutorialService _tutorialService;
    private readonly ISaveLoadService _save;
    private readonly ILevelManager _levelManager;

    private bool _isLevelUpStudiedStarted;
    private bool _isGiveWinsStudied;
    private Coroutine _coroutine;

    public TutorialState(
        GameStateMachine gameStateMachine,
        ICoroutineRunner coroutineRunner,
        IProgressService progressService,
        ITutorialService tutorialService,
        ISaveLoadService saveLoadService,
        ILevelManager levelManager)
    {
        _gameStateMachine = gameStateMachine;
        _coroutineRunner = coroutineRunner;
        _progressService = progressService;
        _tutorialService = tutorialService;
        _save = saveLoadService;
        _levelManager = levelManager;
    }

    public void Enter()
    {
        if (!_progressService.Data.Tutorial.IsLevelUpStudied)
        {
            _levelManager.LevelChanged += OnLevelUp;
            _isLevelUpStudiedStarted = true;
        }

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

        if(_isLevelUpStudiedStarted)
            _levelManager.LevelChanged -= OnLevelUp;
    }

    public IEnumerator TutorialCoroutine()
    {
        while (!_tutorialService.IsReady)        
            yield return null;        

        UnityEngine.Debug.Log($"IsWeaponSwitcherStudied {_progressService.Data.Tutorial.IsWeaponSwitcherStudied}");

        if (!_progressService.Data.Tutorial.IsWeaponSwitcherStudied)
        {
            _tutorialService.Player.StartDrawLine(_tutorialService.StonesSwitcher.transform);

            while (!_tutorialService.StonesSwitcher.IsSelected)
                yield return null;

            _tutorialService.Player.StopDrawLine();
            _progressService.Data.Tutorial.IsWeaponSwitcherStudied = true;
            _save.SaveProgress();
        }

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
        }

        if (!_progressService.Data.Tutorial.IsGameLevelStudied)
        {
            _isGiveWinsStudied = false;
            ObstaclePresenter obstacle = _tutorialService.LevelView_1.Targets[0];

            _tutorialService.Player.StartDrawLine(obstacle.View.Transform);

            while (!obstacle.IsDestroyed)
                yield return null;

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

            while(!_isGiveWinsStudied)
                yield return null;

            _tutorialService.Player.StopDrawLine();

           yield return new WaitForSeconds(0.3f);

            _tutorialService.Hud.TutorialUI.ShowGiveWinsMessage();

            _progressService.Data.Tutorial.IsGameLevelStudied = true;
            _save.SaveProgress();
        }


        _progressService.Data.Tutorial.IsTutorialComplete = true;
        _save.SaveProgress();

        _coroutine = null;

        _gameStateMachine.Enter<GameLoopState>();
    }

    private void OnPlayerGiveWins()
    {
        _tutorialService.Transition2.PlayerGiveWins -= OnPlayerGiveWins;
        _isGiveWinsStudied = true;

    }

    private void OnLevelUp(int obj) => 
        _tutorialService.Hud.TutorialUI.ShowLevelUpMessage();
}