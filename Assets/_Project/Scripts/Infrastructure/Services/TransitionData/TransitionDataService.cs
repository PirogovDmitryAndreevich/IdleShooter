using System;

public class TransitionDataService : ITransitionDataService
{
    public PlayerPresenter Player { get; private set; }

    public event Action PlayerCreated;

    public void SetPlayer(PlayerPresenter player)
    {
        Player = player;

        PlayerCreated?.Invoke();
    }
}