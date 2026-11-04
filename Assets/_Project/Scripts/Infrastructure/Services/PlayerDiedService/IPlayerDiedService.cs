using System;

public interface IPlayerDiedService : IService, IDisposable
{
    event Action<LevelType> PlayerDied;

    void OnUserNoShowedAdv();
    void OnUserShowedAdv();
}