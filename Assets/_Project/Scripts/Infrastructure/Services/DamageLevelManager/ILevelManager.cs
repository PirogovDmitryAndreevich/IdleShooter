using System;

public interface ILevelManager : IService
{
    int CurrentLevel { get; }
    float CurrentScore { get; }
    int PlayerHealth { get; }
    int Wins { get; }
    bool IsMaxLevel { get; }

    event Action<int> LevelChanged;
    event Action MaxLevelReached;
    event Action<float, float> ScoreChanged;
    event Action<int> HealthViewUpdated;
    event Action<int> WinsChanged;

    void AddScore(float score);
    void AddWins(int value);
    void SpendWins(int value);
    float GetCurrentTargetScore();
    float GetLevelProgress();
    float GetNextTargetScore();
    void UpdateHealthView();
}