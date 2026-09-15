using Assets._Project.Scripts.SpawnPoints.Obstacles;
using System.Collections;
using UnityEngine;

public class LevelLoader : ILevelLoader
{
    private readonly IGameFactory _gameFactory;
    private readonly ICoroutineRunner _coroutineRunner;

    public LevelLoader(IGameFactory gameFactory, ICoroutineRunner coroutineRunner)
    {
        _gameFactory = gameFactory;
        _coroutineRunner = coroutineRunner;
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

        LevelPresenter presenter = new LevelPresenter(lobby, null);

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
        LevelView_1 levelView = CreateLevelView<LevelView_1>(position, LevelType.Level_1);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);

        foreach (TargetSpawnPoint targetPoint in levelView.TargetsSpawnPoints)
        {
            _gameFactory.CreateTarget(targetPoint.transform, levelView.transform, level, ObstacleType.Target);

            yield return null;
        }

        operation.Complete(level);
    }

    private IEnumerator InitLevel2(Transform position, LevelLoadOperation operation)
    {
        LevelView_2 levelView = CreateLevelView<LevelView_2>(position, LevelType.Level_2);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);

        foreach (TargetSpawnPoint targetPoint in levelView.TargetsSpawnPoints)
        {
            _gameFactory.CreateTarget(targetPoint.transform, levelView.transform,
                level, ObstacleType.TargetLevel2);

            yield return null;
        }

        foreach (SpinningCylinderSpawnPoint cylinderPoint in levelView.SpinningCylinderSpawnPoint)
        {
            _gameFactory.CreateSpinningCylinder(
                cylinderPoint.transform,
                levelView.transform,
                level, ObstacleType.SpinningCylinder);

            yield return null;
        }

        FlyPlatform platform = _gameFactory.CreateFlyPlatform(
            levelView.FlyPlatformSpawnPoint.transform.position,
            levelView.FlyPlatformSpawnPoint.EndPoint.position,
            levelView.transform);

        GearPresenter gear = _gameFactory.CreateGear(
            levelView.GearSpawnPoint.transform,
            levelView.transform,
            level,
            ObstacleType.Gear);

        gear.SetTriggerToMoving(platform);

        _gameFactory.CreateGearBase(levelView.GearBaseSpawnPoint.transform,
            levelView.transform, level, ObstacleType.GearBase);

        foreach (SpikeCylinderSpawnPoint spikeCylinderSpawnPoint in levelView.SpikeCylinderSpawnPoints)
        {
            _gameFactory.CreateSpikeCylinder(spikeCylinderSpawnPoint.transform,
                levelView.transform, level, ObstacleType.SpikeCylinder);

            yield return null;
        }

        operation.Complete(level);
    }

    private IEnumerator InitLevel3(Transform position, LevelLoadOperation operation)
    {
        LevelView_3 levelView = CreateLevelView<LevelView_3>(position, LevelType.Level_3);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);

        foreach (var soldierSpawnPoint in levelView.SoldierSpawnPoints)
        {
            _gameFactory.CreateSoldierEnemy(
                soldierSpawnPoint.transform,
                soldierSpawnPoint.AttackPoint.transform,
                levelView.PlayerTriggerObserver,
                level,
                levelView.transform);

            yield return null;
        }

        operation.Complete(level);
    }

    private IEnumerator InitLevel4(Transform position, LevelLoadOperation operation)
    {
        LevelView_4 levelView = CreateLevelView<LevelView_4>(position, LevelType.Level_4);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);

        _gameFactory.CreateDemonEnemy(
            levelView.DemonSpawnPoint.transform,
            levelView.PlayerTriggerObserver,
            level,
            levelView.transform);

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

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel6(Transform position, LevelLoadOperation operation)
    {
        LevelView_6 levelView = CreateLevelView<LevelView_6>(position, LevelType.Level_6);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);

        for (int i = 0; i < levelView.ZombiesSpawnPoints.Length; i++)
        {
            _gameFactory.CreateZombieEnemy(
                levelView.ZombiesSpawnPoints[i].transform,
                levelView.PlayerTriggerObserver,
                level,
                levelView.transform);

            yield return null;
        }

        operation.Complete(level);
    }

    private IEnumerator InitLevel7(Transform position, LevelLoadOperation operation)
    {
        LevelView_7 levelView = CreateLevelView<LevelView_7>(position, LevelType.Level_7);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);

        _gameFactory.CreateGhostEnemy(
            levelView.GhostSpawnPoint.transform,
            levelView.PlayerTriggerObserver,
            level,
            levelView.transform);

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel8(Transform position, LevelLoadOperation operation)
    {
        LevelView_8 levelView = CreateLevelView<LevelView_8>(position, LevelType.Level_8);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);

        for (int i = 0; i < levelView.CannonSpawnPoints.Length; i++)
        {
            _gameFactory.CreateCannon(
                levelView.CannonSpawnPoints[i].transform,
                levelView.PlayerTriggerObserver,
                level,
                levelView.transform);

            yield return null;
        }

        operation.Complete(level);

        yield break;
    }

    private IEnumerator InitLevel9(Transform position, LevelLoadOperation operation)
    {
        LevelView_9 levelView = CreateLevelView<LevelView_9>(position, LevelType.Level_9);

        LevelPresenter level = new LevelPresenter(levelView, levelView.Transition);

        Transform levelTransform = levelView.transform;

        for (int i = 0; i < levelView.FlyPlatformSpawnPoints.Length; i++)
        {
            FlyPlatformSpawnPoint PlatformSpawnPoint = levelView.FlyPlatformSpawnPoints[i];

            var platform = _gameFactory.CreateFlyPlatform(
                PlatformSpawnPoint.transform.position,
                PlatformSpawnPoint.EndPoint.transform.position,
                levelTransform);

            GearSpawnPoint gearSpawnPoint = levelView.GearSpawnPoints[i];

            var gear = _gameFactory.CreateGear(
                gearSpawnPoint.transform,
                levelTransform,
                level,
                ObstacleType.Gear9);

            gear.SetTriggerToMoving(platform);

            yield return null;
        }

        for (int i = 0; i < levelView.SpinningCylinderSpawnPoints.Length; i++)
        {
            _gameFactory.CreateSpinningCylinder(
                levelView.SpinningCylinderSpawnPoints[i].transform,
                levelTransform,
                level,
                ObstacleType.SpinningCylinder9);

            yield return null;
        }

        for (int i = 0; i < levelView.GearBaseSpawnPoints.Length; i++)
        {
            GearBaseSpawnPoint point = levelView.GearBaseSpawnPoints[i];

            _gameFactory.CreateGearBase(
                point.transform,
                levelTransform,
                level,
                ObstacleType.GearBase9);
        }

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