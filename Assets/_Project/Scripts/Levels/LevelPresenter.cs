using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelPresenter : IDisposable
{
    private readonly LevelView _levelView;
    private readonly Transition _transition;
    public List<IResetObject> ResetObjects;
    public List<System.IDisposable> Disposables;

    public event Action<LevelType> TransitionEntered;

    public List<IPlayerTarget> PlayerTargets = new List<IPlayerTarget>();

    public LevelType LevelType =>
        _levelView.LevelType;

    public Transform ExitPoint =>
        _levelView.ExitPoint;

    public Transition Transition =>
        _transition;

    public LevelPresenter(LevelView levelView, Transition transition)
    {
        ResetObjects = new List<IResetObject>();
        Disposables = new List<System.IDisposable>();

        _levelView = levelView;
        _transition = transition;

        if (transition != null)
            _transition.TransitionEntered += OnTransitionEntered;
    }

    public void ResetLevel()
    {
        foreach (IResetObject obj in ResetObjects)
            obj.ResetObject();

        _transition.SetState(false);
    }

    public void SetActive(bool shouldBeActive)
    {
        _levelView.SetActive(shouldBeActive);
    }

    public void Dispose()
    {
        foreach (System.IDisposable disposable in Disposables)
            disposable?.Dispose();

        _transition.TransitionEntered -= OnTransitionEntered;
    }

    public void RegisterPlayerTarget(IPlayerTarget playerTarget)
    {
        if (!PlayerTargets.Contains(playerTarget))
            PlayerTargets.Add(playerTarget);
    }

    public bool CheckCompleted()
    {
        foreach (var target in PlayerTargets)
            if (!target.IsDestroyed)
                return false;

        return true;
    }

    public void OpenTransition() => 
        _transition.SetState(true);

    private void OnTransitionEntered() =>
        TransitionEntered?.Invoke(LevelType);
}