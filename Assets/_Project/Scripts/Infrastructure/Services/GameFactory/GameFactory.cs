using UnityEngine;

public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;
    private readonly IInputService _inputService;
    private readonly IStaticDataService _staticDataService;
    private readonly IProgressService _progressService;
    private readonly IPoolService _poolService;
    private readonly IWeaponDataService _weaponDataService;
    private readonly IGameSaveDataService _gameSaveDataService;
    private readonly IProjectilePool _projectilePool;
    private readonly IWarpService _warpService;
    private readonly ITransitionDataService _transitionDataService;
    private readonly ICoroutineRunner _coroutineRunner;
    private PlayerPresenter _player;
    WeaponSwitcher _weaponSwitcher;

    public GameFactory(IAssets assets, IInputService inputService, IStaticDataService staticDataService,
        IProgressService progressService, IPoolService poolService, IWeaponDataService weaponDataService,
        IGameSaveDataService gameSaveDataService, IProjectilePool projectilePool, IWarpService warpService,
ITransitionDataService transitionDataService, ICoroutineRunner coroutineRunner)
    {
        _assets = assets;
        _inputService = inputService;
        _staticDataService = staticDataService;
        _progressService = progressService;
        _poolService = poolService;
        _weaponDataService = weaponDataService;
        _gameSaveDataService = gameSaveDataService;
        _projectilePool = projectilePool;
        _warpService = warpService;
        _transitionDataService = transitionDataService;
        _coroutineRunner = coroutineRunner;

        _projectilePool.Register(ProjectileType.Stone, () => CreateStoneProjectile());
        _projectilePool.Register(ProjectileType.Minigun, () => CreateMinigunProjectile());
        _projectilePool.Register(ProjectileType.Rocket, () => CreateRocketProjectile());

        _poolService.RegisterFactory(() => CreateBulletHole());
        _poolService.RegisterFactory(() => CreateHitEffect());
        _poolService.RegisterFactory(() => CreateRPGMuzzle());
        _poolService.RegisterFactory(() => CreateRocketExplosion());
        _poolService.RegisterFactory(() => CreateSphereObstacle());
    }

    public PlayerPresenter CreatePlayer(UnityEngine.Vector3 at)
    {
        _weaponSwitcher = new WeaponSwitcher(this, _weaponDataService);

        PlayerBehaviour player = _assets.Instantiate(AssetsPath.CharacterPath, at)
            .GetComponent<PlayerBehaviour>();

        player.View.Construct();
        CameraController camera = CreatePlayerCamera(player.View, UnityEngine.Camera.main);
        PlayerModel model = new PlayerModel(_staticDataService, _progressService);


        _player = new PlayerPresenter(model, player.View, _inputService, camera, _weaponSwitcher);

        Hud hud = CreateHud(_player);

        player.Initialize(_player);

        _warpService.RegisterPlayer(_player);
        _transitionDataService.SetPlayer(_player);

        return _player;
    }

    public Transition CreateTransition(Transform at, LevelType type)
    {
        Transition transition = _assets.Instantiate(AssetsPath.TransitionPath, at: at.position)
            .GetComponent<Transition>();

        transition.Construct(type);

        transition.transform.rotation = at.rotation;

        _warpService.RegisterSpawnPoint(type, transition.PlayerSpawnPoint);

        return transition;
    }

    public Lobby CreateLobby()
    {
        GameStaticData gameData = _staticDataService.GetGameData();

        Lobby lobby = _assets.Instantiate(
            _staticDataService.GetLevelView<Lobby>(LevelType.Lobby),
            at: gameData.LobbySpawnPosition);

        _warpService.RegisterSpawnPoint(LevelType.Lobby, lobby.PlayerSpawnPoint.transform);

        return lobby;
    }

    public T CreateLevel<T>(Transform spawnPoint, LevelType type) where T : LevelView
    {
        T level = _assets.Instantiate(
            _staticDataService.GetLevelView<T>(type),
            at: spawnPoint.position);

        level.transform.rotation = spawnPoint.rotation;

        return level;
    }

    public SwitchWeaponButton CreateWeaponSwitcher(WeaponType type, Vector3 position, Transform parent)
    {
        SwitchWeaponButton switcher = _assets.Instantiate(AssetsPath.WeaponSwitcherPath, at: position)
            .GetComponent<SwitchWeaponButton>();

        WeaponStaticData weaponData = _staticDataService.GetWeaponData(type);

        switcher.Construct(_weaponSwitcher, _weaponDataService, type, weaponData);

        switcher.transform.SetParent(parent);

        return switcher;
    }

    public TargetPresenter CreateTarget(Transform point, Transform parent, LevelPresenter level, ObstacleType target)
    {
        TargetView view = _assets.Instantiate(AssetsPath.TargetPath, at: point.position)
            .GetComponent<TargetView>();

        Transform transform = view.transform;

        transform.localScale = point.localScale;
        transform.localRotation = point.rotation;

        transform.SetParent(parent);

        TargetModel model =
            new TargetModel(_staticDataService.ForObstacle(target));

        TargetPresenter presenter = new TargetPresenter(model, view);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public SpinningCylinderPresenter CreateSpinningCylinder(Transform point, Transform parent,
        LevelPresenter level, ObstacleType spinningCylinder)
    {
        SpinningCylinderView view = _assets.Instantiate(AssetsPath.SpinningCylinderPath, at: point.position)
            .GetComponent<SpinningCylinderView>();

        Transform transform = view.transform;

        transform.localScale = point.localScale;
        transform.localRotation = point.rotation;

        transform.SetParent(parent);

        SpinningCylinderModel model =
            new SpinningCylinderModel(
                _staticDataService.ForObstacle(spinningCylinder));

        SpinningCylinderPresenter presenter = new SpinningCylinderPresenter(model, view);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public GearPresenter CreateGear(Transform point, Transform parent,
        LevelPresenter level, ObstacleType gear)
    {
        GearView view = _assets.Instantiate(AssetsPath.GearPath, at: point.position)
            .GetComponent<GearView>();

        Transform transform = view.transform;

        transform.localScale = point.localScale;
        transform.localRotation = point.rotation;

        transform.SetParent(parent);

        GearModel model = new GearModel(_staticDataService.ForObstacle(gear));

        GearPresenter presenter = new GearPresenter(model, view);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public GearBasePresenter CreateGearBase(Transform point, Transform parent,
        LevelPresenter level, ObstacleType gearBase)
    {
        GearBaseView view = _assets.Instantiate(AssetsPath.GearBasePath, at: point.position)
            .GetComponent<GearBaseView>();

        Transform transform = view.transform;

        transform.localScale = point.localScale;
        transform.localRotation = point.rotation;

        transform.SetParent(parent);

        GearBaseModel model = new GearBaseModel(_staticDataService.ForObstacle(gearBase));
        GearBasePresenter presenter = new GearBasePresenter(model, view);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public SpikeCylinderPresenter CreateSpikeCylinder(Transform point, Transform parent,
        LevelPresenter level, ObstacleType spikeCylinder)
    {
        SpikeCylinderView view = _assets.Instantiate(AssetsPath.SpikeCylinderPath, at: point.position)
            .GetComponent<SpikeCylinderView>();

        Transform transform = view.transform;

        transform.localScale = point.localScale;
        transform.localRotation = point.rotation;

        transform.SetParent(parent);

        SpikeCylinderModel model = new SpikeCylinderModel(_staticDataService.ForObstacle(spikeCylinder));
        SpikeCylinderPresenter presenter = new SpikeCylinderPresenter(model, view);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public FlyPlatform CreateFlyPlatform(Vector3 startPosition, Vector3 endPosition, Transform parent)
    {
        FlyPlatform platform = _assets.Instantiate(AssetsPath.FlyPlatformPath, startPosition)
            .GetComponent<FlyPlatform>();

        platform.Construct(endPosition);

        platform.transform.SetParent(parent);

        return platform;
    }

    public WeaponPresenter CreateWeapon(WeaponType type)
    {
        WeaponView view = _assets.Instantiate(_staticDataService.GetWeaponView(type));
        WeaponModel model = new WeaponModel(_staticDataService.GetWeaponData(type));

        WeaponPresenter presenter = new WeaponPresenter(model, view, UnityEngine.Camera.main,
            _staticDataService.GetGameData(), _projectilePool, _poolService);

        return presenter;
    }

    public StoneProjectile CreateStoneProjectile()
    {
        StoneProjectile stone = _assets.Instantiate(AssetsPath.StoneProjectilePath)
            .GetComponent<StoneProjectile>();

        stone.Construct(_projectilePool);

        return stone;
    }

    public MinigunProjectile CreateMinigunProjectile()
    {
        MinigunProjectile projectile = _assets.Instantiate(AssetsPath.MinigunProjectilePath)
            .GetComponent<MinigunProjectile>();

        projectile.Construct(_projectilePool, _poolService);

        return projectile;
    }

    public SoldierPresenter CreateSoldierEnemy(Transform at, Transform attackPoint,
        PlayerTriggerObserver playerTriggerObserver, LevelPresenter level, Transform parent)
    {
        SoldierView view = _assets.Instantiate(AssetsPath.SoldierEnemyPath, at.position)
            .GetComponent<SoldierView>();

        view.transform.SetParent(parent);

        view.transform.localRotation = at.localRotation;

        EnemyModel model = new EnemyModel(_staticDataService.ForEnemy(EnemyType.Soldier));
        EnemyShooter shooter = new EnemyShooter(model, view.Weapon, _projectilePool);

        SoldierStateMachine stateMachine =
            new SoldierStateMachine(view, shooter, model, at, attackPoint, _coroutineRunner);

        SoldierPresenter presenter =
            new SoldierPresenter(model, view, playerTriggerObserver, stateMachine);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public DemonPresenter CreateDemonEnemy(Transform at, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent)
    {
        DemonView view = _assets.Instantiate(AssetsPath.DemonBossPath, at.position)
            .GetComponent<DemonView>();

        view.transform.SetParent(parent);

        EnemyModel model = new EnemyModel(_staticDataService.ForEnemy(EnemyType.Demon));

        view.NavMeshAgent.speed = model.MoveSpeed;
        view.NavMeshAgent.angularSpeed = model.RotateSpeed;

        DemonPresenter presenter = new DemonPresenter(model, view, at, playerTriggerObserver);

        view.DemonBehaviour.SetPresenter(presenter);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public Warp CreateWarp(LevelType type, Vector3 at, Transform parent)
    {
        Warp warp = _assets.Instantiate(AssetsPath.WarpPath, at)
            .GetComponent<Warp>();

        warp.Construct(type);

        _warpService.RegisterWarp(warp);

        warp.transform.SetParent(parent);

        return warp;
    }

    public SphereObstacle CreateSphereObstacle()
    {
        SphereObstacle sphere = _assets.Instantiate(AssetsPath.SphereObstaclePath)
            .GetComponent<SphereObstacle>();

        return sphere;
    }

    public SphereSpawner CreateSphereSpawner(Transform at, SphereSpawnPoint[] spawnPoints)
    {
        SphereSpawner spawner = _assets.Instantiate(AssetsPath.SphereSpawnerPath, at)
            .GetComponent<SphereSpawner>();

        spawner.Construct(_poolService, spawnPoints);

        return spawner;
    }

    public ZombiePresenter CreateZombieEnemy(Transform spawnPoint, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent)
    {
        ZombieView view = _assets.Instantiate(AssetsPath.ZombiePath, spawnPoint.position)
            .GetComponent<ZombieView>();

        view.transform.SetParent(parent);

        EnemyModel model = new EnemyModel(_staticDataService.ForEnemy(EnemyType.Zombie));

        view.NavMeshAgent.speed = model.MoveSpeed;
        view.NavMeshAgent.angularSpeed = model.RotateSpeed;

        ZombiePresenter presenter = new ZombiePresenter(view, model, spawnPoint, playerTriggerObserver);

        view.ZombieBehaviour.SetPresenter(presenter);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public GhostPresenter CreateGhostEnemy(Transform spawnPoint, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent)
    {
        GhostView view = _assets.Instantiate(AssetsPath.GhostPath, spawnPoint.position)
            .GetComponent<GhostView>();

        view.transform.SetParent(parent);

        EnemyModel model = new EnemyModel(_staticDataService.ForEnemy(EnemyType.Ghost));

        view.NavMeshAgent.speed = model.MoveSpeed;
        view.NavMeshAgent.angularSpeed = model.RotateSpeed;

        GhostPresenter presenter = new GhostPresenter(view, model, spawnPoint, playerTriggerObserver);

        view.GhostBehaviour.SetPresenter(presenter);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    private Hud CreateHud(PlayerPresenter playerPresenter)
    {
        var hud = _assets.Instantiate(AssetsPath.HudPath)
            .GetComponent<Hud>();

        hud.Construct(playerPresenter);

        return hud;
    }

    private CameraController CreatePlayerCamera(PlayerView player, UnityEngine.Camera camera)
    {
        return new CameraController(player, camera, _inputService, _gameSaveDataService,
           _staticDataService.GetGameData());
    }

    private BulletHole CreateBulletHole()
    {
        BulletHole hole = _assets.Instantiate(AssetsPath.BulletHolePath)
            .GetComponent<BulletHole>();

        hole.Construct(_poolService);

        return hole;
    }

    private BulletHitEffect CreateHitEffect()
    {
        BulletHitEffect effect = _assets.Instantiate(AssetsPath.BulletHitEffectPath)
            .GetComponent<BulletHitEffect>();

        effect.Construct(_poolService);

        return effect;
    }

    private RocketProjectile CreateRocketProjectile()
    {
        RocketProjectile projectile = _assets.Instantiate(AssetsPath.RocketProjectilePath)
            .GetComponent<RocketProjectile>();

        projectile.Construct(_projectilePool, _poolService);

        return projectile;
    }

    private RocketExplosion CreateRocketExplosion()
    {
        RocketExplosion explosion = _assets.Instantiate(AssetsPath.RocketExplosionPath)
            .GetComponent<RocketExplosion>();

        explosion.Construct(_poolService);

        return explosion;
    }

    private RPGMuzzleEffect CreateRPGMuzzle()
    {
        RPGMuzzleEffect muzzle = _assets.Instantiate(AssetsPath.RPGMuzzlePath)
            .GetComponent<RPGMuzzleEffect>();

        muzzle.Construct(_poolService);

        return muzzle;
    }
}