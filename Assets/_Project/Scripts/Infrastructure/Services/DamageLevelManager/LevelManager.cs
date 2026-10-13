using System;

public class LevelManager : ILevelManager
{
    private readonly IProgressService _progress;
    private readonly PlayerLevelsStaticData[] _levelData;

    public LevelManager(IProgressService progress, IStaticDataService staticData)
    {
        _progress = progress;
        _levelData = staticData.GetGameData().PlayerLevelsData;
    }

    public event Action<float, float> ScoreChanged;
    public event Action<int> LevelChanged;
    public event Action<int> HealthViewUpdated;
    public event Action MaxLevelReached;
    public event Action<int> WinsChanged;

    public int CurrentLevel => _progress.Data.PlayerData.Level;
    public float CurrentScore => _progress.Data.PlayerData.CurrentScore;
    public int PlayerHealth => _progress.Data.PlayerData.Health;
    public int Wins => _progress.Data.PlayerData.Wins;

    public bool IsMaxLevel =>
        CurrentLevel >= _levelData.Length;

    public void AddScore(float score)
    {
        if (score <= 0 || IsMaxLevel)
            return;

        _progress.Data.PlayerData.CurrentScore += score;

        UpdateLevel();

        ScoreChanged?.Invoke(
            CurrentScore,
             GetNextTargetScore());
    }

    public void ChangeWins(int value)
    {
        _progress.Data.PlayerData.Wins += value;
        WinsChanged?.Invoke(Wins);
    }

    public float GetCurrentTargetScore()
    {
        return _levelData[CurrentLevel - 1].TargetScore;
    }

    public float GetNextTargetScore()
    {
        if (IsMaxLevel)
            return GetCurrentTargetScore();

        return _levelData[CurrentLevel].TargetScore;
    }

    public float GetLevelProgress()
    {
        if (IsMaxLevel)
            return 1f;

        float currentTarget = GetCurrentTargetScore();
        float nextTarget = GetNextTargetScore();

        float levelScore = nextTarget - currentTarget;

        if (levelScore <= 0)
            return 1f;

        return (CurrentScore - currentTarget) / levelScore;
    }

    public void UpdateHealthView()
    {
        HealthViewUpdated?.Invoke(
            _progress.Data.PlayerData.Health);
    }

    private void UpdateLevel()
    {
        int previousLevel = CurrentLevel;

        while (!IsMaxLevel &&
               CurrentScore >= GetNextTargetScore())
        {
            _progress.Data.PlayerData.Level++;

            UpdatePlayerHealth();
        }

        if (CurrentLevel != previousLevel)
            LevelChanged?.Invoke(CurrentLevel);

        if (IsMaxLevel && previousLevel != CurrentLevel)
            MaxLevelReached?.Invoke();
    }

    private void UpdatePlayerHealth()
    {
        int health =
            _levelData[CurrentLevel - 1].Health;

        _progress.Data.PlayerData.Health = health;
    }
}