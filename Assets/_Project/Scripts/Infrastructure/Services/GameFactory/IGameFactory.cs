using UnityEngine;

public interface IGameFactory : IService
{
    PlayerPresenter CreatePlayer(Vector3 at);
    Transition CreateTransition(Transform at, LevelType type);
    T CreateLevel<T>(Transform spawnPoint, LevelType type) where T : LevelView;
    Lobby CreateLobby();
    WeaponPresenter CreateWeapon(WeaponType type);
    SwitchWeaponButton CreateWeaponSwitcher(WeaponType type, Vector3 position, Transform parent);
    TargetPresenter CreateTarget(Transform point, Transform parent, LevelPresenter level, ObstacleType target);
    SpinningCylinderPresenter CreateSpinningCylinder(Transform point, Transform parent, LevelPresenter level,
        ObstacleType spinningCylinder);
    GearPresenter CreateGear(Transform point, Transform parent, LevelPresenter level, ObstacleType gear);
    SpikeCylinderPresenter CreateSpikeCylinder(Transform point, Transform parent, LevelPresenter level, ObstacleType spikeCylinder);
    GearBasePresenter CreateGearBase(Transform point, Transform parent, LevelPresenter level, ObstacleType gearBase);
    FlyPlatform CreateFlyPlatform(Vector3 startPosition, Vector3 endPosition, Transform parent);
    SoldierPresenter CreateSoldierEnemy(Transform at, Transform attackPoint, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent);
    DemonPresenter CreateDemonEnemy(Transform at, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent);
    Warp CreateWarp(LevelType type, Vector3 at, Transform parent);
}