public interface IMetricsService : IService
{
    void BoughtWeapon(WeaponType weaponType);
    void PlayerLevelUp();
    void ReachedGameLevel(LevelType levelType);
}