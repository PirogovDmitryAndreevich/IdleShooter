using System;
using System.Collections.Generic;

public class TransitionDataService : ITransitionDataService
{
    private Dictionary<LevelType, LevelPresenter> _levels;

    private List<LevelType> _enteredLevels;

    public TransitionDataService()
    {
        _levels = new Dictionary<LevelType, LevelPresenter>();
        _enteredLevels = new List<LevelType>();
    }

    public PlayerPresenter Player { get; private set; }

    public event Action PlayerCreated;

    public void SetPlayer(PlayerPresenter player)
    {
        Player = player;

        PlayerCreated?.Invoke();
    }

    public void ResetEnteredLevels()
    {
        foreach (LevelType level in _enteredLevels)        
            _levels[level].ResetLevel();        

        _enteredLevels.Clear();
    }

    public void RegisterLevel(LevelPresenter level)
    {
        if (_levels.ContainsKey(level.LevelType))
            return;

        _levels[level.LevelType] = level;

        level.TransitionEntered += OnLevelEntered;
    }

    public void Dispose()
    {
        foreach (var level in _levels)
            level.Value.TransitionEntered -= OnLevelEntered;
    }

    private void OnLevelEntered(LevelType type)
    {
        _enteredLevels.Add(type);
    }    
}