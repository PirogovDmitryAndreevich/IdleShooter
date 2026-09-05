using System;

public interface ITransitionDataService : IService
{
    PlayerPresenter Player { get; }

    event Action PlayerCreated;

    void SetPlayer(PlayerPresenter player);
}