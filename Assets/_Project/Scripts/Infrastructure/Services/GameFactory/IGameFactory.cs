public interface IGameFactory : IService
{
    void CreatePlayer(UnityEngine.Vector3 at);
    public WeaponPresenter CreateWeapon(WeaponType type);
    SelectWeaponButton CreateWeaponSelectButton(UnityEngine.Transform parent);
}