using System;
using System.Collections.Generic;

public class LevelPresenter : IDisposable
{
    private readonly LevelView _levelView;

    public List<IResetObject> ResetObjects;
    public List<IDisposable> Disposables;

    public LevelPresenter(LevelView levelView)
    {
        ResetObjects = new List<IResetObject>();
        Disposables = new List<IDisposable>();

        _levelView = levelView;
        _levelView.Construct(this);
    }

    public void ResetLevel()
    {
        foreach (IResetObject obj in ResetObjects)
            obj.ResetObject();
    }

    public void Dispose()
    {
        foreach (IDisposable disposable in Disposables)
            disposable.Dispose();
    }
}