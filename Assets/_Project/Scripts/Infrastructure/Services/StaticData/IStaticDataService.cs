public interface IStaticDataService : IService
{
    public void Load();

    PlayerStaticData ForPlayer();
    WeaponView GetWeaponView(WeaponType type);
    WeaponStaticData GetWeaponData(WeaponType type);
    GameStaticData GetGameData();
}