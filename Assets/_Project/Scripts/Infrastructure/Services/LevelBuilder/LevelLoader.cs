using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : ILevelLoader
{
    private readonly IGameFactory _gameFactory;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly ITransitionDataService _transitionDataService;

    private Lobby _lobby;

    public LevelLoader(IGameFactory gameFactory, ICoroutineRunner coroutineRunner, ITransitionDataService transitionDataService)
    {
        _gameFactory = gameFactory;
        _coroutineRunner = coroutineRunner;
        _transitionDataService = transitionDataService;
    }

    public LevelLoadOperation InitLevel(LevelType type, Transform position)
    {
        LevelLoadOperation operation =
            new LevelLoadOperation();

        _coroutineRunner.StartCoroutine(
            LoadLevel(type, position, operation));

        return operation;
    }

    private IEnumerator LoadLevel(
        LevelType type,
        Transform position,
        LevelLoadOperation operation)
    {
        switch (type)
        {
            case LevelType.Lobby:
                yield return InitLobby(position, operation);
                break;
            case LevelType.Level_1:
                yield return InitLevel1(position, operation);
                break;
            case LevelType.Level_2:
                yield return InitLevel2(position, operation);
                break;
            case LevelType.Level_3:
                yield return InitLevel3(position, operation);
                break;
            case LevelType.Level_4:
                yield return InitLevel4(position, operation);
                break;
            case LevelType.Level_5:
                yield return InitLevel5(position, operation);
                break;
            case LevelType.Level_6:
                yield return InitLevel6(position, operation);
                break;
            case LevelType.Level_7:
                yield return InitLevel7(position, operation);
                break;
            case LevelType.Level_8:
                yield return InitLevel8(position, operation);
                break;
            case LevelType.Level_9:
                yield return InitLevel9(position, operation);
                break;
            case LevelType.Level_10:
                yield return InitLevel10(position, operation);
                break;
            case LevelType.Level_11:
                yield return InitLevel11(position, operation);
                break;
            default:
                Debug.LogError(
                    $"LevelLoader: unsupported level type {type}");

                yield break;
        }
    }

    private IEnumerator InitLobby(Transform position, LevelLoadOperation operation)
    {
        WarpService warpStarter = new WarpService();

        Lobby lobby = _gameFactory.CreateLobby();

        _lobby = lobby;

        LevelPresenter presenter = new LevelPresenter(lobby, null);
        _transitionDataService.RegisterLevel(presenter);

        PlayerPresenter player = _gameFactory.CreatePlayer(lobby.PlayerSpawnPoint.transform.position);

        foreach (WeaponSwitcherSpawnPoint spawnPoint in lobby.WeaponSwitcherSpawnPoint)
        {
            _gameFactory.CreateWeaponSwitcher(
                spawnPoint.WeaponType,
                spawnPoint.transform.position,
                lobby.transform);
        }

        foreach (WarpSpawnPoint warpSpawnPoint in lobby.WarpSpawnPoint)
        {
            _gameFactory.CreateWarp(warpSpawnPoint.Type,
                 warpSpawnPoint.transform.position,
                 lobby.transform);
        }

        presenter.Disposables.Add(warpStarter);

        operation.Complete(presenter);

        yield break;
    }

    private IEnumerator InitLevel1(Transform position, LevelLoadOperation operation)
    {
        LevelView_1 levelView = _gameFactory.CreateLevel<LevelView_1>(position, LevelType.Level_1);
        levelView.Construct(_lobby.TransitionFor1Level);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        List<ObstaclePresenter> targets = new List<ObstaclePresenter>();

        foreach (ObstacleSpawnPoint obstacles in levelView.ObstacleSpawnPoints)
        {
            var target =_gameFactory.CreateObstacle(obstacles.transform, levelView.transform, level, obstacles.Type);

            targets.Add(target);

            yield return null;
        }

        levelView.Targets = targets.ToArray();

        operation.Complete(level);
    }

    private IEnumerator InitLevel2(Transform position, LevelLoadOperation operation)
    {
        LevelView_2 levelView = CreateLevelView<LevelView_2>(position, LevelType.Level_2);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        foreach (ObstacleSpawnPoint obstacles in levelView.ObstacleSpawnPoints)
        {
            ObstaclePresenter obstacle = _gameFactory.CreateObstacle(obstacles.transform, levelView.transform,
                level, obstacles.Type);

            yield return null;
        }

        FlyPlatform platform = _gameFactory.CreateFlyPlatform(
            levelView.FlyPlatformSpawnPoint.transform.position,
            levelView.FlyPlatformSpawnPoint.EndPoint.position,
            levelView.transform,
            level);

        ObstaclePresenter gear = _gameFactory.CreateObstacle(
            levelView.GearForPlatformMoving.transform,
            levelView.transform,
            level,
            ObstacleType.Gear);

        GearMoveToPlatform gearLogic = new GearMoveToPlatform(
            _coroutineRunner,
            gear.View,
            platform,
            levelView.FlyPlatformSpawnPoint.GearSpeed);

        level.ResetObjects.Add(gearLogic);
        level.Disposables.Add(gearLogic);

        operation.Complete(level);
    }

    private IEnumerator InitLevel3(Transform position, LevelLoadOperation operation)
    {
        LevelView_3 levelView = CreateLevelView<LevelView_3>(position, LevelType.Level_3);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        foreach (var soldierSpawnPoint in levelView.SoldierSpawnPoints)
        {
            SoldierPresenter soldier = _gameFactory.CreateSoldierEnemy(
                soldierSpawnPoint.transform,
                soldierSpawnPoint.AttackPoint.transform,
                levelView.PlayerTriggerObserver,
                level,
                levelView.transform,
                EnemyType.Soldier);

            level.RegisterPlayerTarget(soldier);

            yield return null;
        }

        operation.Complete(level);
    }

    private IEnumerator InitLevel4(Transform position, LevelLoadOperation operation)
    {
        LevelView_4 levelView = CreateLevelView<LevelView_4>(position, LevelType.Level_4);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        DemonPresenter demon = _gameFactory.CreateDemonEnemy(
            levelView.DemonSpawnPoint.transform,
            levelView.PlayerTriggerObserver,
            level,
            levelView.transform);

        level.RegisterPlayerTarget(demon);

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel5(Transform position, LevelLoadOperation operation)
    {
        LevelView_5 levelView = CreateLevelView<LevelView_5>(position, LevelType.Level_5);

        _gameFactory.CreateSphereSpawner(
            levelView.transform,
            levelView.SphereSpawnPoints);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel6(Transform position, LevelLoadOperation operation)
    {
        LevelView_6 levelView = CreateLevelView<LevelView_6>(position, LevelType.Level_6);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        for (int i = 0; i < levelView.ZombiesSpawnPoints.Length; i++)
        {
            ZombiePresenter zombie = _gameFactory.CreateZombieEnemy(
                levelView.ZombiesSpawnPoints[i].transform,
                levelView.PlayerTriggerObserver,
                level,
                levelView.transform,
                EnemyType.Zombie);

            level.RegisterPlayerTarget(zombie);

            yield return null;
        }

        operation.Complete(level);
    }

    private IEnumerator InitLevel7(Transform position, LevelLoadOperation operation)
    {
        LevelView_7 levelView = CreateLevelView<LevelView_7>(position, LevelType.Level_7);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        GhostPresenter ghost = _gameFactory.CreateGhostEnemy(
            levelView.GhostSpawnPoint.transform,
            levelView.PlayerTriggerObserver,
            level,
            levelView.transform);

        level.RegisterPlayerTarget(ghost);

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel8(Transform position, LevelLoadOperation operation)
    {
        LevelView_8 levelView = CreateLevelView<LevelView_8>(position, LevelType.Level_8);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        for (int i = 0; i < levelView.CannonSpawnPoints.Length; i++)
        {
            CannonPresenter cannon = _gameFactory.CreateCannon(
                levelView.CannonSpawnPoints[i].transform,
                levelView.PlayerTriggerObserver,
                level,
                levelView.transform);

            level.RegisterPlayerTarget(cannon);

            yield return null;
        }

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel9(Transform position, LevelLoadOperation operation)
    {
        LevelView_9 levelView = CreateLevelView<LevelView_9>(position, LevelType.Level_9);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        Transform levelTransform = levelView.transform;

        for (int i = 0; i < levelView.PlatformsAndGears.Length; i++)
        {
            FlyPlatformSpawnPoint PlatformSpawnPoint = levelView.PlatformsAndGears[i].PlatformSpawnPoint;

            FlyPlatform platform = _gameFactory.CreateFlyPlatform(
                PlatformSpawnPoint.transform.position,
                PlatformSpawnPoint.EndPoint.transform.position,
                levelTransform,
                level);

            ObstacleSpawnPoint gearSpawnPoint = levelView.PlatformsAndGears[i].GearSpawnPoint;

            var gear = _gameFactory.CreateObstacle(
                gearSpawnPoint.transform,
                levelTransform,
                level,
                gearSpawnPoint.Type);

            GearMoveToPlatform gearLogic = new GearMoveToPlatform(
                _coroutineRunner,
                gear.View,
                platform,
                PlatformSpawnPoint.GearSpeed);

            level.ResetObjects.Add(gearLogic);
            level.Disposables.Add(gearLogic);

            yield return null;
        }

        for (int i = 0; i < levelView.ObstacleSpawnPoints.Length; i++)
        {
            _gameFactory.CreateObstacle(
                levelView.ObstacleSpawnPoints[i].transform,
                levelTransform,
                level,
                levelView.ObstacleSpawnPoints[i].Type);

            yield return null;
        }

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel10(Transform position, LevelLoadOperation operation)
    {
        LevelView_10 levelView = CreateLevelView<LevelView_10>(position, LevelType.Level_10);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        Transform levelTransform = levelView.transform;

        for (int i = 0; i < levelView.FlyPlatformSpawnPoints.Length; i++)
        {
            FlyPlatformSpawnPoint PlatformSpawnPoint = levelView.FlyPlatformSpawnPoints[i];

            _gameFactory.CreateFlyPlatform(
                PlatformSpawnPoint.transform.position,
                PlatformSpawnPoint.EndPoint.transform.position,
                levelTransform,
                level);

            yield return null;
        }

        for (int i = 0; i < levelView.PlatformsAndGears.Length; i++)
        {
            FlyPlatformSpawnPoint platformSpawnPoint = levelView.PlatformsAndGears[i].PlatformSpawnPoint;

            FlyPlatform platform = _gameFactory.CreateFlyPlatform(
                platformSpawnPoint.transform.position,
                platformSpawnPoint.EndPoint.transform.position,
                levelTransform,
                level);

            ObstacleSpawnPoint gearSpawnPoint = levelView.PlatformsAndGears[i].GearSpawnPoint;

            ObstaclePresenter gear = _gameFactory.CreateObstacle(
                gearSpawnPoint.transform,
                levelView.transform,
                level,
                gearSpawnPoint.Type);

            GearMoveToPlatform gearLogic = new GearMoveToPlatform(
                _coroutineRunner,
                gear.View,
                platform,
                platformSpawnPoint.GearSpeed);

            level.ResetObjects.Add(gearLogic);
            level.Disposables.Add(gearLogic);

            yield return null;
        }

        for (int i = 0; i < levelView.CannonsSpawnPoints.Length; i++)
        {
            CannonPresenter cannon = _gameFactory.CreateCannon(
                levelView.CannonsSpawnPoints[i].transform,
                levelView.PlayerTriggerObserver,
                level,
                levelView.transform);

            yield return null;
        }

        FinalPhaseForLevel_10 finalPhase = levelView.ForFinalPhase;

        for (int i = 0; i < finalPhase.ZombieSpawnPoints.Length; i++)
        {
            ZombiePresenter zombie = _gameFactory.CreateZombieEnemy(
                finalPhase.ZombieSpawnPoints[i].transform,
                finalPhase.PlayerTriggerObserver,
                level,
                levelView.transform,
                EnemyType.Zombie10);

            level.RegisterPlayerTarget(zombie);

            yield return null;
        }

        for (int i = 0; i < finalPhase.SoldierSpawnPoints.Length; i++)
        {
            SoldierSpawnPoint soldierSpawnPoint = finalPhase.SoldierSpawnPoints[i];

            SoldierPresenter soldier = _gameFactory.CreateSoldierEnemy(
                soldierSpawnPoint.transform,
                soldierSpawnPoint.AttackPoint.transform,
                finalPhase.PlayerTriggerObserver,
                level,
                levelView.transform,
                EnemyType.Soldier10);

            level.RegisterPlayerTarget(soldier);

            yield return null;
        }

        _gameFactory.CreateSphereSpawner(
            levelView.transform,
            levelView.SphereSpawnPoints);

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel11(Transform position, LevelLoadOperation operation)
    {
        LevelView_11 levelView = CreateLevelView<LevelView_11>(position, LevelType.Level_11);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);
        _transitionDataService.RegisterLevel(level);

        RobotPresenter robot = _gameFactory.CreateRobotEnemy(
            levelView.RobotSpawnPoint.transform,
            levelView.PlayerTriggerObserver,
            level,
            levelView.transform);

        level.RegisterPlayerTarget(robot);

        operation.Complete(level);

        yield break;
    }

    private T CreateLevelView<T>(Transform position, LevelType type) where T : LevelView
    {
        Transition transition = InitTransition(position, type);

        T levelView = _gameFactory.CreateLevel<T>(transition.ExitPoint, type);

        transition.transform.SetParent(levelView.transform);

        levelView.Construct(transition);

        return levelView;
    }

    private Transition InitTransition(Transform position, LevelType type) =>
        _gameFactory.CreateTransition(position, type);
}