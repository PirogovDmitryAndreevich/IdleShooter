using System;
using System.Collections.Generic;

public class TransitionDataService : ITransitionDataService
{
    private Dictionary<LevelType, LevelPresenter> _levels;

    private List<LevelType> _enteredLevels;
    private readonly IInterstitialAdvService _interstitialAdvService;

    public LevelPresenter CurrentLevel { get; private set; }

    public TransitionDataService(IInterstitialAdvService interstitialAdvService)
    {
        _levels = new Dictionary<LevelType, LevelPresenter>();
        _enteredLevels = new List<LevelType>();

        _interstitialAdvService = interstitialAdvService;
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

    public void OpenNextLevel()
    {
        if (CurrentLevel == null)
            return;

        if (TryGetNextLevel(out LevelPresenter nextLevel))
            nextLevel.OpenTransition();
    }

    private void OnLevelEntered(LevelType type)
    {
        CurrentLevel = _levels[type];
        _enteredLevels.Add(type);

        _interstitialAdvService.TryShowAdv();
    }

    private bool TryGetNextLevel(out LevelPresenter nextLevel)
    {
        int nextIndex = (int)CurrentLevel.LevelType + 1;
        LevelType nextLevelType = (LevelType)nextIndex;

        return _levels.TryGetValue(nextLevelType, out nextLevel);
    }
}