public interface IGameFactory : IService
{
    void CreatePlayer(UnityEngine.Vector3 at);
    Lobby CreateLobby();
    public WeaponPresenter CreateWeapon(WeaponType type);
    void CreateWeaponSwitcher(WeaponType type, UnityEngine.Vector3 position);
}