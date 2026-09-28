using System;

public interface ILevelManager : IService
{
    int CurrentLevel { get; }
    float CurrentScore { get; }
    bool IsMaxLevel { get; }

    event Action<int> LevelChanged;
    event Action MaxLevelReached;
    event Action<float, float> ScoreChanged;

    void AddScore(float score);
    float GetCurrentTargetScore();
    float GetLevelProgress();
    float GetNextTargetScore();
}