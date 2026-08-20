public interface IStaticDataService : IService
{
    public void Load();

    PlayerStaticData ForPlayer();
    WeaponView GetWeaponView(WeaponType type);
    WeaponStaticData GetWeaponData(WeaponType type);
    GameStaticData GetGameData();
    EnemyStaticData ForEnemy(EnemyType type);
    LevelsStaticData ForLevel(LevelType type);
    T GetLevelView<T>(LevelType type) where T : LevelView;
}