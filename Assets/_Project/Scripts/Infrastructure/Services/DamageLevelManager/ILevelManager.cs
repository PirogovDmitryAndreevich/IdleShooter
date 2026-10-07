using System;

public interface ILevelManager : IService
{
    int CurrentLevel { get; }
    float CurrentScore { get; }
    int PlayerHealth { get; }
    bool IsMaxLevel { get; }

    event Action<int> LevelChanged;
    event Action MaxLevelReached;
    event Action<float, float> ScoreChanged;
    event Action<int> HealthViewUpdated;

    void AddScore(float score);
    float GetCurrentTargetScore();
    float GetLevelProgress();
    float GetNextTargetScore();
    void UpdateHealthView();
}