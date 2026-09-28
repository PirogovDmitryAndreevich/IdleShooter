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
    public event Action MaxLevelReached;

    public int CurrentLevel => _progress.Data.PlayerData.Level;
    public float CurrentScore => _progress.Data.PlayerData.CurrentScore;

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

    private void UpdateLevel()
    {
        int previousLevel = CurrentLevel;

        while (!IsMaxLevel &&
               CurrentScore >= GetNextTargetScore())
        {
            _progress.Data.PlayerData.Level++;
        }

        if (CurrentLevel != previousLevel)
            LevelChanged?.Invoke(CurrentLevel);

        if (IsMaxLevel && previousLevel != CurrentLevel)
            MaxLevelReached?.Invoke();
    }
}