using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string PlayerStaticDataPath = "StaticData/PlayerStaticData";
    private const string WeaponViewsPath = "StaticData/WeaponView/WeaponViews";
    private const string WeaponsDataPath = "StaticData/WeaponsData";
    private const string GameDataPath = "StaticData/GameData/GameStaticData";

    private PlayerStaticData _playerStaticData;
    private GameStaticData _gameStaticData;
    private Dictionary<WeaponType, WeaponView> WeaponViews = new();
    private Dictionary<WeaponType, WeaponStaticData> WeaponsData = new();

    public void Load()
    {
        _playerStaticData = Resources.Load<PlayerStaticData>(PlayerStaticDataPath);

        _gameStaticData = Resources.Load<GameStaticData>(GameDataPath);

        WeaponViews = Resources.Load<WeaponViewsData>(WeaponViewsPath)
            .WeaponViews
            .ToDictionary(x => x.Type, x => x);

        WeaponsData = Resources.LoadAll<WeaponStaticData>(WeaponsDataPath)
            .ToDictionary(x => x.Type, x => x);
    }

    public PlayerStaticData ForPlayer() =>
        _playerStaticData;

    public WeaponView GetWeaponView(WeaponType type) =>
        WeaponViews.TryGetValue(type, out WeaponView view)
        ? view
        : null;

    public WeaponStaticData GetWeaponData(WeaponType type) =>
        WeaponsData.TryGetValue(type, out WeaponStaticData data)
        ? data 
        : null;

    public GameStaticData GetGameData() => 
        _gameStaticData;
}