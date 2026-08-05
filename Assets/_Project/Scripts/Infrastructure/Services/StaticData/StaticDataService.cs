using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string PlayerStaticDataPath = "StaticData/PlayerStaticData";

    PlayerStaticData _playerStaticData;

    public void Load()
    {
        _playerStaticData = Resources.Load<PlayerStaticData>(PlayerStaticDataPath);
    }

    public PlayerStaticData ForPlayer() =>
        _playerStaticData;
}