using System.Collections.Generic;
using UnityEngine;
using YG;

public class MetricsService : IMetricsService
{
    private readonly IProgressService _progress;

    public MetricsService(IProgressService progress)
    {
        _progress = progress;
    }

    public void PlayerLevelUp()
    {
        string playerLevel = _progress.Data.PlayerData.Level.ToString();

        YG2.MetricaSend("PlayerLevelReached", playerLevel, new Dictionary<string, object>
            {
                { "Time", Time.time },
                { "PlayerDamage", _progress.Data.PlayerData.Damage },
                { "Wins", _progress.Data.PlayerData.Wins }
            });
    }

    public void ReachedGameLevel(LevelType levelType)
    {
        string level = ((int)levelType).ToString();

        YG2.MetricaSend("GameLevelReached", level, new Dictionary<string, object>
            {
                { "Time", Time.time },
                { "PlayerDamage", _progress.Data.PlayerData.Damage },
                { "PlayerLevel", _progress.Data.PlayerData.Level },
                { "Wins", _progress.Data.PlayerData.Wins }
            });
    }

    public void BoughtWeapon(WeaponType weaponType)
    {
        string weapon = weaponType.ToString();

        YG2.MetricaSend("BoughtWeapon", weapon, new Dictionary<string, object>
            {
                { "Time", Time.time },
                { "PlayerDamage", _progress.Data.PlayerData.Damage },
                { "PlayerLevel", _progress.Data.PlayerData.Level },
                { "Wins", _progress.Data.PlayerData.Wins }
            });
    }
}