public interface IGameFactory : IService
{
    void CreatePlayer(UnityEngine.Vector3 at);
    public WeaponPresenter CreateWeapon(WeaponType type);
    void CreateWeaponSwitcher(WeaponType type, UnityEngine.Vector3 position);
}