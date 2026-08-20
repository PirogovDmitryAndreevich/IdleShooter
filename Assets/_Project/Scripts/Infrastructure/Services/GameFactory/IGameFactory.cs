using UnityEngine;

public interface IGameFactory : IService
{
    void CreatePlayer(UnityEngine.Vector3 at);
    Transition CreateTransition(Transform at);
    T CreateLevel<T>(Transform spawnPoint, LevelType type) where T : LevelView;
    Lobby CreateLobby();
    public WeaponPresenter CreateWeapon(WeaponType type);
    void CreateWeaponSwitcher(WeaponType type, UnityEngine.Vector3 position);
    Target CreateTarget(UnityEngine.Vector3 position);
    SpinningCylinder CreateSpinningCylinder(UnityEngine.Vector3 position);
    Gear CreateGear(UnityEngine.Vector3 position);
    SpikeCylinder CreateSpikeCylinder(UnityEngine.Vector3 position);
    GearBase CreateGearBase(UnityEngine.Vector3 position);
}