using System;
using System.Collections;

public class LevelController : IDisposable, ILevelController
{
    private readonly ILevelLoader _levelLoader;
    private readonly ICoroutineRunner _coroutineRunner;

    private readonly LevelRegistry _registry;
    private readonly LevelWindow _window;

    private LevelType _currentLevel;
    private LevelType _requestedLevel;

    private bool _isChangingLevel;

    public LevelController(
        ILevelLoader levelLoader,
        ICoroutineRunner coroutineRunner)
    {
        _levelLoader = levelLoader;
        _coroutineRunner = coroutineRunner;

        _registry = new LevelRegistry();
        _window = new LevelWindow(_registry);
    }

    public void Initialize()
    {
        _coroutineRunner.StartCoroutine(
            InitializeLevels());
    }

    private IEnumerator InitializeLevels()
    {
        LevelLoadOperation lobbyOperation =
            _levelLoader.InitLevel(
                LevelType.Lobby,
                null);

        yield return lobbyOperation;

        RegisterLevel(
            lobbyOperation.Presenter);

        LevelLoadOperation level1Operation =
            _levelLoader.InitLevel(
                LevelType.Level_1,
                lobbyOperation.Presenter.ExitPoint);

        yield return level1Operation;

        RegisterLevel(
            level1Operation.Presenter);

        _currentLevel = LevelType.Lobby;
        _requestedLevel = LevelType.Lobby;

        _window.Update(_currentLevel);
    }

    private void RegisterLevel(
        LevelPresenter presenter)
    {
        _registry.Add(presenter);

        presenter.TransitionEntered +=
            OnTransitionEntered;
    }

    private void OnTransitionEntered(
        LevelType levelType)
    {
        if (_currentLevel == levelType)
            return;

        _requestedLevel = levelType;

        if (_isChangingLevel)
            return;

        _coroutineRunner.StartCoroutine(
            ChangeLevelRoutine());
    }

    private IEnumerator ChangeLevelRoutine()
    {
        _isChangingLevel = true;

        while (_currentLevel != _requestedLevel)
        {
            yield return LoadLevelWindow(
                _requestedLevel);
        }

        _isChangingLevel = false;
    }

    private IEnumerator LoadLevelWindow(
        LevelType currentLevel)
    {
        LevelType? previousLevel =
            _window.GetPreviousLevel(currentLevel);

        LevelType? nextLevel =
            _window.GetNextLevel(currentLevel);

        
        if (!_registry.Contains(currentLevel))
        {
            LevelPresenter previous =
                GetLevelForLoading(
                    previousLevel);

            LevelLoadOperation operation =
                _levelLoader.InitLevel(
                    currentLevel,
                    previous.ExitPoint);

            yield return operation;

            RegisterLevel(
                operation.Presenter);
        }

        if (nextLevel.HasValue &&
            !_registry.Contains(nextLevel.Value))
        {
            LevelPresenter current =
                _registry.Get(currentLevel);

            LevelLoadOperation operation =
                _levelLoader.InitLevel(
                    nextLevel.Value,
                    current.ExitPoint);

            yield return operation;

            RegisterLevel(
                operation.Presenter);
        }

        _currentLevel = currentLevel;

        _window.Update(_currentLevel);
    }

    private LevelPresenter GetLevelForLoading(
        LevelType? previousLevel)
    {
        if (!previousLevel.HasValue)
            throw new InvalidOperationException(
                $"Cannot load level because previous level is undefined.");

        return _registry.Get(
            previousLevel.Value);
    }

    public void Dispose()
    {
        foreach (LevelPresenter presenter in _registry.All)
        {
            presenter.TransitionEntered -=
                OnTransitionEntered;

            presenter.Dispose();
        }

        _registry.Clear();
    }
}