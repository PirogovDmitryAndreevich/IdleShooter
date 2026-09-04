using System;
using System.Collections;
using System.Collections.Generic;

public class LevelRegister : IDisposable
{
    private readonly ILevelLoader _levelLoader;
    private readonly LevelSequence _sequence;

    private readonly Dictionary<LevelType, LevelPresenter> _levels = new();

    public event Action<LevelType> LevelTransitionEntered;

    public IEnumerable<LevelPresenter> All =>
        _levels.Values;

    public LevelRegister(
        ILevelLoader levelLoader,
        LevelSequence sequence)
    {
        _levelLoader = levelLoader;
        _sequence = sequence;
    }

    public IEnumerator EnsureWindow(
        LevelType current)
    {
        LevelType? previous =
            _sequence.GetPrevious(current);

        LevelType? next =
            _sequence.GetNext(current);

        if (previous.HasValue)
        {
            yield return EnsureLevel(
                previous.Value);
        }

        yield return EnsureLevel(
            current);

        if (next.HasValue)
        {
            yield return EnsureLevel(
                next.Value);
        }
    }

    private IEnumerator EnsureLevel(
        LevelType type)
    {
        if (_levels.ContainsKey(type))
            yield break;

        LevelType? previous =
            _sequence.GetPrevious(type);

        if (previous.HasValue)
        {
            yield return EnsureLevel(
                previous.Value);
        }

        LevelPresenter previousPresenter = null;

        if (previous.HasValue)
        {
            previousPresenter =
                _levels[previous.Value];
        }

        LevelLoadOperation operation =
            _levelLoader.InitLevel(
                type,
                previousPresenter?.ExitPoint);

        yield return operation;

        Register(
            operation.Presenter);
    }

    public bool Contains(LevelType type) =>
        _levels.ContainsKey(type);

    public LevelPresenter Get(LevelType type) =>
        _levels[type];

    public void Dispose()
    {
        foreach (LevelPresenter presenter
                 in _levels.Values)
        {
            presenter.TransitionEntered -= OnTransitionEntered;
            presenter.Dispose();
        }

        _levels.Clear();
    }

    private void Register(LevelPresenter presenter)
    {
        _levels[presenter.LevelType] = presenter;

        presenter.TransitionEntered += OnTransitionEntered;
    }

    private void OnTransitionEntered(LevelType type)
    {
        LevelTransitionEntered?.Invoke(type);
    }
}