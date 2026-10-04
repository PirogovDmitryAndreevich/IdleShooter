using UnityEngine;

public interface IGameFactory : IService
{
    void SetLevelController(ILevelController levelController);
    PlayerPresenter CreatePlayer(Vector3 at);
    Transition CreateTransition(Transform at, LevelType type);
    T CreateLevel<T>(Transform spawnPoint, LevelType type) where T : LevelView;
    Lobby CreateLobby();
    WeaponPresenter CreateWeapon(WeaponType type);
    SwitchWeaponButton CreateWeaponSwitcher(WeaponType type, Vector3 position, Transform parent);
    ObstaclePresenter CreateObstacle(Transform point, Transform parent, LevelPresenter level, ObstacleType type);
    FlyPlatform CreateFlyPlatform(Vector3 startPosition, Vector3 endPosition, Transform parent, LevelPresenter level);
    SoldierPresenter CreateSoldierEnemy(Transform at, Transform attackPoint, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent, EnemyType type);
    CannonPresenter CreateCannon(Transform at, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent);
    DemonPresenter CreateDemonEnemy(Transform at, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent);
    RobotPresenter CreateRobotEnemy(Transform at, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent);
    Warp CreateWarp(LevelType type, Vector3 at, Transform parent);
    SphereObstacle CreateSphereObstacle();
    SphereSpawner CreateSphereSpawner(Transform at, SphereSpawnPoint[] spawnPoints);
    ZombiePresenter CreateZombieEnemy(Transform spawnPoint, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent, EnemyType type);
    GhostPresenter CreateGhostEnemy(Transform spawnPoint, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent);
}