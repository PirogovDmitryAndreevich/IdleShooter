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

    public LevelType LevelType =>
        _levelView.LevelType;

    public Transform ExitPoint =>
        _levelView.ExitPoint;

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

    private void OnTransitionEntered() =>
        TransitionEntered?.Invoke(LevelType);
}