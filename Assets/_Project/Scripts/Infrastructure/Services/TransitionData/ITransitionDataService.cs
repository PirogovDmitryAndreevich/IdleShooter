using System;

public interface ITransitionDataService : IService, IDisposable
{
    PlayerPresenter Player { get; }

    event Action PlayerCreated;

    void SetPlayer(PlayerPresenter player);

    void ResetEnteredLevels();

    void RegisterLevel(LevelPresenter level);


}