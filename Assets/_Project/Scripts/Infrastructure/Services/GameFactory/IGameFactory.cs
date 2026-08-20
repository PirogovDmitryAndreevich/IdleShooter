public interface IGameFactory : IService
{
    void CreatePlayer(UnityEngine.Vector3 at);
    Lobby CreateLobby();
    public WeaponPresenter CreateWeapon(WeaponType type);
    void CreateWeaponSwitcher(WeaponType type, UnityEngine.Vector3 position);
    SpinningCylinder CreateSpinningCylinder(UnityEngine.Vector3 position);
    Gear CreateGear(UnityEngine.Vector3 position);
    GearBase CreateGearBase(UnityEngine.Vector3 position);
}