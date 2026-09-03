using UnityEngine;

public class LevelLoadOperation : CustomYieldInstruction
{
    public LevelPresenter Presenter { get; private set; }

    public bool IsCompleted { get; private set; }

    public override bool keepWaiting =>
        !IsCompleted;

    internal void Complete(LevelPresenter presenter)
    {
        Presenter = presenter;
        IsCompleted = true;
    }
}