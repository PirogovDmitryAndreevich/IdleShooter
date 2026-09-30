using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService : IStaticDataService
{
    private const string PlayerStaticDataPath = "StaticData/PlayerStaticData";     
    private const string GameDataPath = "StaticData/GameData/GameStaticData";

    private PlayerStaticData _playerStaticData;
    private GameStaticData _gameStaticData;
    private ShootUIEffectAnimationSettings _shootAnimationSettings;
    private Dictionary<WeaponType, WeaponView> WeaponViews = new();
    private Dictionary<WeaponType, WeaponStaticData> WeaponsData = new();
    private Dictionary<EnemyType, EnemyStaticData> Enemies = new();
    private Dictionary<LevelType, LevelsStaticData> LevelsData = new();
    private Dictionary<LevelType, LevelView> LevelsView = new();
    private Dictionary<ObstacleType, ObstacleStaticData> Obstacles = new();

    public void Load()
    {
        _gameStaticData = Resources.Load<GameStaticData>(GameDataPath);

        _playerStaticData = Resources.Load<PlayerStaticData>(PlayerStaticDataPath);

        _shootAnimationSettings = _gameStaticData.ShootUIEffectAnimationSettings;

        WeaponViews = _gameStaticData.WeaponsView
            .WeaponViews
            .ToDictionary(x => x.Type, x => x);

        WeaponsData = _gameStaticData.WeaponsStaticData
            .ToDictionary(x => x.Type, x => x);

        Enemies = _gameStaticData.EnemiesStaticData
            .ToDictionary(x => x.Type, x => x);

        Obstacles = _gameStaticData.Obstacles
            .ToDictionary(x => x.Type, x => x);

        LevelsData = _gameStaticData.LevelsData
            .ToDictionary(x => x.Type, x => x);

        LevelsView = _gameStaticData.LevelViews
            .ToDictionary(x => x.LevelType, x => x);
    }

    public PlayerStaticData ForPlayer() =>
        _playerStaticData;

    public ShootUIEffectAnimationSettings GetShootAnimationSettings() =>
        _shootAnimationSettings;

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

    public EnemyStaticData ForEnemy(EnemyType type) =>
        Enemies.TryGetValue(type, out EnemyStaticData data)
        ? data
        : null;

    public LevelsStaticData ForLevel(LevelType type) =>
        LevelsData.TryGetValue(type, out LevelsStaticData data)
        ? data
        : null;

    public ObstacleStaticData ForObstacle(ObstacleType type) =>
        Obstacles.TryGetValue(type, out ObstacleStaticData data)
        ? data
        : null;

    public T GetLevelView<T>(LevelType type) where T : LevelView =>
        LevelsView.TryGetValue(type, out LevelView levelView)
        ? levelView as T
        : null;
}