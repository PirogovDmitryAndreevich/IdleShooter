using UnityEngine;

public interface IGameFactory : IService
{
    void CreatePlayer(Vector3 at);
    Transition CreateTransition(Transform at, Transform parent);
    T CreateLevel<T>(Transform spawnPoint, LevelType type) where T : LevelView;
    Lobby CreateLobby();
    WeaponPresenter CreateWeapon(WeaponType type);
    SwitchWeaponButton CreateWeaponSwitcher(WeaponType type, Vector3 position, Transform parent);
    TargetPresenter CreateTarget(Transform point, Transform parent, LevelPresenter level, ObstacleType target);
    SpinningCylinderPresenter CreateSpinningCylinder(Transform point, Transform parent, LevelPresenter level, ObstacleType spinningCylinder);
    GearPresenter CreateGear(Transform point, Transform parent, LevelPresenter level, ObstacleType gear);
    SpikeCylinderPresenter CreateSpikeCylinder(Transform point, Transform parent, LevelPresenter level, ObstacleType spikeCylinder);
    GearBasePresenter CreateGearBase(Transform point, Transform parent, LevelPresenter level, ObstacleType gearBase);
    FlyPlatform CreateFlyPlatform(UnityEngine.Vector3 startPosition, UnityEngine.Vector3 endPosition);
}