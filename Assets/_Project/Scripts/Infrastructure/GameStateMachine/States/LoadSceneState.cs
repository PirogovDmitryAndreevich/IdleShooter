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
        Lobby lobby = InitLobby();

        Transition transition1 = InitTransition(lobby, lobby.LevelType);

        LevelView_1 level_1 = InitLevel1(transition1);

        Transition transition2 = InitTransition(level_1, level_1.LevelType);

        LevelView_2 levelView_2 = InitLevel2(transition2);

        Transition transition3 = InitTransition(levelView_2, levelView_2.LevelType);

        _weaponDataService.ForInitialize();
    }

    private Lobby InitLobby()
    {
        Lobby lobby = _gameFactory.CreateLobby();

        _gameFactory.CreatePlayer(lobby.PlayerSpawnPoint.transform.position);

        foreach (WeaponSwitcherSpawnPoint spawnPoint in lobby.WeaponSwitcherSpawnPoint)
            _gameFactory.CreateWeaponSwitcher(
                spawnPoint.WeaponType,
                spawnPoint.transform.position,
                lobby.transform);

        return lobby;
    }

    private Transition InitTransition(LevelView levelView, LevelType type) =>
        _gameFactory.CreateTransition(levelView.ExitPoint, levelView.transform);

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
            levelView.FlyPlatformSpawnPoint.EndPoint.position);

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

    private T CreateLevel<T>(Transition transition, LevelType type) where T : LevelView =>
        _gameFactory.CreateLevel<T>(transition.ExitPoint, type);
}