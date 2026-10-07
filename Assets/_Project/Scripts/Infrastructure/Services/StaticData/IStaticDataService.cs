public interface IStaticDataService : IService
{
    public void Load();

    PlayerStaticData ForPlayer();
    ShootUIEffectAnimationSettings GetShootAnimationSettings();
    WeaponView GetWeaponView(WeaponType type);
    WeaponStaticData GetWeaponData(WeaponType type);
    GameStaticData GetGameData();
    EnemyStaticData ForEnemy(EnemyType type);
    LevelsStaticData ForLevel(LevelType type);
    ObstacleStaticData ForObstacle(ObstacleType type);
    T GetLevelView<T>(LevelType type) where T : LevelView;
    SoundsEffects GetSoundEffects();
}