using Assets._Project.Scripts.SpawnPoints.Obstacles;
using System.Collections.Generic;

public class LoadSceneState : IPayloadState<string>
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly IGameFactory _gameFactory;
    private readonly IWeaponDataService _weaponDataService;
    private readonly IStaticDataService _staticData;

    public LoadSceneState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        IGameFactory gameFactory, IWeaponDataService weaponDataService, IStaticDataService staticData)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _gameFactory = gameFactory;
        _weaponDataService = weaponDataService;
        _staticData = staticData;
    }

    public void Enter(string payload)
    {
        _sceneLoader.Load(payload, OnLoaded);
    }

    public void Exit()
    {

    }

    private void OnLoaded()
    {
        WarpStarter warpStarter = new WarpStarter();

        Lobby lobby = InitLobby(warpStarter);

        Transition transition1 = InitTransition(lobby, LevelType.Level_1, warpStarter);

        LevelView_1 level_1 = InitLevel1(transition1);

        Transition transition2 = InitTransition(level_1, LevelType.Level_2, warpStarter);

        LevelView_2 levelView_2 = InitLevel2(transition2);

        Transition transition3 = InitTransition(levelView_2, LevelType.Level_3, warpStarter);

        LevelView_3 levelView_3 = InitLevel3(transition3);

        Transition transition4 = InitTransition(levelView_3, LevelType.Level_4, warpStarter);

        LevelView_4 levelView_4 = InitLevel4(transition4);

        Transition transition_5 = InitTransition(levelView_4, LevelType.Level_5, warpStarter);

        _weaponDataService.ForInitialize();
    }

    private Lobby InitLobby(WarpStarter warpStarter)
    {
        Lobby lobby = _gameFactory.CreateLobby();

        LevelPresenter presenter = new LevelPresenter(lobby);

        PlayerPresenter player = _gameFactory.CreatePlayer(lobby.PlayerSpawnPoint.transform.position);

        foreach (WeaponSwitcherSpawnPoint spawnPoint in lobby.WeaponSwitcherSpawnPoint)
            _gameFactory.CreateWeaponSwitcher(
                spawnPoint.WeaponType,
                spawnPoint.transform.position,
                lobby.transform);

        List<Warp> warps = new();

        foreach (WarpSpawnPoint warpSpawnPoint in lobby.WarpSpawnPoint)
        {
            Warp warp;

            warp = _gameFactory.CreateWarp(warpSpawnPoint.Type,
                warpSpawnPoint.transform.position,
                lobby.transform);

            warps.Add(warp);
        }

        warpStarter.Initialize(warps, player);

        presenter.Disposables.Add(warpStarter);

        return lobby;
    }

    private Transition InitTransition(LevelView levelView, LevelType type, WarpStarter warpStarter)
    {
        var transition = _gameFactory.CreateTransition(levelView.ExitPoint, levelView.transform, type);

        warpStarter.RegisterTransition(transition);

        return transition;
    }

    private LevelView_1 InitLevel1(Transition transition)
    {
        LevelView_1 levelView = CreateLevel<LevelView_1>(transition, LevelType.Level_1);

        LevelPresenter level = new LevelPresenter(levelView);

        foreach (TargetSpawnPoint targetPoint in levelView.TargetsSpawnPoints)
            _gameFactory.CreateTarget(targetPoint.transform, levelView.transform, level, ObstacleType.Target);

        return levelView;
    }

    private LevelView_2 InitLevel2(Transition transition1)
    {
        LevelView_2 levelView = CreateLevel<LevelView_2>(transition1, LevelType.Level_2);

        LevelPresenter level = new LevelPresenter(levelView);

        foreach (TargetSpawnPoint targetPoint in levelView.TargetsSpawnPoints)
            _gameFactory.CreateTarget(targetPoint.transform, levelView.transform,
                level, ObstacleType.TargetLevel2);

        foreach (SpinningCylinderSpawnPoint cylinderPoint in levelView.SpinningCylinderSpawnPoint)
            _gameFactory.CreateSpinningCylinder(
                cylinderPoint.transform,
                levelView.transform,
                level, ObstacleType.SpinningCylinder);

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
            _gameFactory.CreateSpikeCylinder(spikeCylinderSpawnPoint.transform,
                levelView.transform, level, ObstacleType.SpikeCylinder);

        return levelView;
    }

    private LevelView_3 InitLevel3(Transition transition)
    {
        LevelView_3 levelView = CreateLevel<LevelView_3>(transition, LevelType.Level_3);

        LevelPresenter level = new LevelPresenter(levelView);

        foreach (var soldierSpawnPoint in levelView.SoldierSpawnPoints)
            _gameFactory.CreateSoldierEnemy(
                soldierSpawnPoint.transform,
                soldierSpawnPoint.AttackPoint.transform,
                levelView.PlayerTriggerObserver,
                level,
                levelView.transform);

        return levelView;
    }

    private LevelView_4 InitLevel4(Transition transition)
    {
        LevelView_4 levelView = CreateLevel<LevelView_4>(transition, LevelType.Level_4);

        LevelPresenter level = new LevelPresenter(levelView);

        _gameFactory.CreateDemonBoss(
            levelView.DemonSpawnPoint.transform,
            levelView.PlayerTriggerObserver,
            level,
            levelView.transform);

        return levelView;
    }

    private T CreateLevel<T>(Transition transition, LevelType type) where T : LevelView =>
        _gameFactory.CreateLevel<T>(transition.ExitPoint, type);
}