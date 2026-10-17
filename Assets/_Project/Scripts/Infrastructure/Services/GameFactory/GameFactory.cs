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
    private readonly IUIPoolService _uIPoolService;
    private readonly IWarpService _warpService;
    private readonly ITransitionDataService _transitionDataService;
    private readonly ILevelManager _levelManager;
    private readonly IInterstitialAdvService _interstitialAdvService;
    private readonly ISettingsManager _settingsManager;
    private readonly IRewardAdvService _rewardAdvService;
    private readonly IWindowsService _windowsService;
    private readonly IGiftService _giftService;
    private readonly ITimeRewardManager _timeRewardManager;
    private readonly IMetricsService _metrics;
    private readonly ICrownsManager _crownsManager;
    private readonly ITutorialService _tutorialService;

    private ILevelController _levelController;
    private PlayerPresenter _player;
    private WeaponSwitcher _weaponSwitcher;

    public GameFactory(IAssets assets, IInputService inputService, IStaticDataService staticDataService,
        IProgressService progressService, IPoolService poolService, IWeaponDataService weaponDataService,
        IGameSaveDataService gameSaveDataService, IProjectilePool projectilePool, IUIPoolService UIPoolService,
        IWarpService warpService, ITransitionDataService transitionDataService, ILevelManager levelManager,
        IInterstitialAdvService interstitialAdvService, ISettingsManager audioManager,
        IRewardAdvService rewardAdvService, IWindowsService windowsService,
        IGiftService giftService, ITimeRewardManager timeRewardManager, IMetricsService metricsService,
        ICrownsManager crownsManager, ITutorialService tutorialService)
    {
        _assets = assets;
        _inputService = inputService;
        _staticDataService = staticDataService;
        _progressService = progressService;
        _poolService = poolService;
        _weaponDataService = weaponDataService;
        _gameSaveDataService = gameSaveDataService;
        _projectilePool = projectilePool;
        _uIPoolService = UIPoolService;
        _warpService = warpService;
        _transitionDataService = transitionDataService;
        _levelManager = levelManager;
        _interstitialAdvService = interstitialAdvService;
        _settingsManager = audioManager;
        _rewardAdvService = rewardAdvService;
        _windowsService = windowsService;
        _giftService = giftService;
        _timeRewardManager = timeRewardManager;
        _metrics = metricsService;
        _crownsManager = crownsManager;
        _tutorialService = tutorialService;

        _projectilePool.Register(ProjectileType.Stone, () => CreateStoneProjectile());
        _projectilePool.Register(ProjectileType.Minigun, () => CreateMinigunProjectile());
        _projectilePool.Register(ProjectileType.Rocket, () => CreateRocketProjectile());
        _projectilePool.Register(ProjectileType.Cannon, () => CreateCannonProjectile());

        _poolService.RegisterFactory(() => CreateBulletHole());
        _poolService.RegisterFactory(() => CreateHitEffect());
        _poolService.RegisterFactory(() => CreateRPGMuzzle());
        _poolService.RegisterFactory(() => CreateRocketExplosion());
        _poolService.RegisterFactory(() => CreateSphereObstacle());

        _uIPoolService.RegisterFactory(() => CreateShootUIEffect());
    }

    public void SetLevelController(ILevelController levelController) =>
        _levelController = levelController;

    public PlayerPresenter CreatePlayer(UnityEngine.Vector3 at)
    {
        _weaponSwitcher = new WeaponSwitcher(this, _weaponDataService);

        PlayerBehaviour player = _assets.Instantiate(AssetsPath.CharacterPath, at)
            .GetComponent<PlayerBehaviour>();

        player.View.Construct();
        player.View.LineDraw.Construct(player.View.transform);
        CameraController camera = CreatePlayerCamera(player.View, UnityEngine.Camera.main);
        PlayerModel model = new PlayerModel(_staticDataService, _progressService);

        DamageManager damageManager = new DamageManager(_progressService);
        PlayerHealth playerHealth = new PlayerHealth(_levelManager);

        _player = new PlayerPresenter(model, player.View, _inputService, camera,
            _weaponSwitcher, damageManager, playerHealth, _levelManager);

        Hud hud = CreateHud(_player);

        player.Initialize(_player);

        _warpService.RegisterPlayer(_player);
        _transitionDataService.SetPlayer(_player);

        _tutorialService.SetPlayerPresenter(_player);
        _tutorialService.SetHud(hud);
        CreateBackgroundMusic();

        return _player;
    }

    public Transition CreateTransition(Transform at, LevelType type)
    {
        Transition transition = _assets.Instantiate(AssetsPath.TransitionPath, at: at.position)
            .GetComponent<Transition>();

        transition.Construct(type, _staticDataService.ForLevel(type), _levelController,
            _levelManager, _metrics);

        transition.transform.rotation = at.rotation;

        _warpService.RegisterSpawnPoint(type, transition.PlayerSpawnPoint);

        if (type == LevelType.Level_2)
            _tutorialService.SetTransition2(transition);

        return transition;
    }

    public Lobby CreateLobby()
    {
        GameStaticData gameData = _staticDataService.GetGameData();

        Lobby lobby = _assets.Instantiate(
            _staticDataService.GetLevelView<Lobby>(LevelType.Lobby),
            at: gameData.LobbySpawnPosition);

        lobby.LeaderBoard.Construct(_crownsManager);
        lobby.PassiveShootingReward.Construct(_rewardAdvService, _progressService);
        lobby.Gift.Construct(_windowsService, _timeRewardManager);

        lobby.Construct(lobby.LobbyTransition);

        lobby.Transition.Construct(LevelType.Lobby, null, _levelController, _levelManager, _metrics);

        _warpService.RegisterSpawnPoint(LevelType.Lobby, lobby.PlayerSpawnPoint.transform);

        lobby.TransitionFor1Level.Construct(LevelType.Level_1,
            _staticDataService.ForLevel(LevelType.Level_1),
            _levelController, _levelManager, _metrics);


        _warpService.RegisterSpawnPoint(LevelType.Level_1, lobby.TransitionFor1Level.PlayerSpawnPoint);

        _tutorialService.SetPassiveShooter(lobby.PassiveShooting);

        return lobby;
    }

    public T CreateLevel<T>(Transform spawnPoint, LevelType type) where T : LevelView
    {
        T level = _assets.Instantiate(
            _staticDataService.GetLevelView<T>(type),
            at: spawnPoint.position);

        level.transform.rotation = spawnPoint.rotation;

        if (type == LevelType.Level_1)
            _tutorialService.SetLevelView_1(level as LevelView_1);

        return level;
    }

    public SwitchWeaponButton CreateWeaponSwitcher(WeaponType type, Vector3 position, Transform parent)
    {
        SwitchWeaponButton switcher = _assets.Instantiate(AssetsPath.WeaponSwitcherPath, at: position)
            .GetComponent<SwitchWeaponButton>();

        WeaponStaticData weaponData = _staticDataService.GetWeaponData(type);

        switcher.Construct(_weaponSwitcher, _weaponDataService, type, weaponData);

        switcher.transform.SetParent(parent);

        if (type == WeaponType.Stones)
            _tutorialService.SetStonesWeaponSwitcher(switcher);

        return switcher;
    }

    public ObstaclePresenter CreateObstacle(Transform point, Transform parent, LevelPresenter level, ObstacleType type)
    {
        ObstacleStaticData data = _staticDataService.ForObstacle(type);

        ObstacleView view = _assets.Instantiate(data.AssetPath, at: point.position)
            .GetComponent<ObstacleView>();

        Transform transform = view.transform;

        transform.localScale = point.localScale;
        transform.localRotation = point.rotation;

        transform.SetParent(parent);

        view.SoundsHandler.Construct(_settingsManager);

        ObstacleModel model = new ObstacleModel(data);

        ObstaclePresenter presenter = new ObstaclePresenter(view, model, type, _crownsManager);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public FlyPlatform CreateFlyPlatform(Vector3 startPosition, Vector3 endPosition, Transform parent, LevelPresenter level)
    {
        FlyPlatform platform = _assets.Instantiate(AssetsPath.FlyPlatformPath, startPosition)
            .GetComponent<FlyPlatform>();

        platform.Construct(endPosition);

        platform.transform.SetParent(parent);

        level.ResetObjects.Add(platform);

        return platform;
    }

    public WeaponPresenter CreateWeapon(WeaponType type)
    {
        WeaponView view = _assets.Instantiate(_staticDataService.GetWeaponView(type));
        WeaponModel model = new WeaponModel(_staticDataService.GetWeaponData(type));

        view.SoundHandler.Construct(_settingsManager);

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
        PlayerTriggerObserver playerTriggerObserver, LevelPresenter level, Transform parent, EnemyType type)
    {
        SoldierView view = _assets.Instantiate(AssetsPath.SoldierEnemyPath, at.position)
            .GetComponent<SoldierView>();

        WeaponStaticData weaponStaticData = _staticDataService.GetWeaponData(view.Weapon.Type);

        view.Weapon.SoundHandler.Construct(_settingsManager);

        view.transform.SetParent(parent);

        view.transform.localRotation = at.localRotation;

        view.SoundsHandler.Construct(_settingsManager);

        EnemyModel model = new EnemyModel(_staticDataService.ForEnemy(type));
        EnemyShooter shooter = new EnemyShooter(model, view.Weapon, weaponStaticData, _projectilePool);

        SoldierPresenter presenter =
            new SoldierPresenter(model, view, at, attackPoint,
            shooter, playerTriggerObserver, _crownsManager);

        view.Behaviour.SetPresenter(presenter);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public CannonPresenter CreateCannon(Transform at, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent)
    {
        CannonView view = _assets.Instantiate(AssetsPath.CannonPath, at.position)
            .GetComponent<CannonView>();

        WeaponStaticData weaponStaticData = _staticDataService.GetWeaponData(view.Weapon.Type);

        view.transform.SetParent(parent);

        view.transform.localRotation = at.localRotation;

        view.SoundsHandler.Construct(_settingsManager);
        // оружие и персонаж используют один обработчик
        //view.Weapon.SoundHandler.Construct(_audioManager);

        EnemyModel model = new EnemyModel(_staticDataService.ForEnemy(EnemyType.Cannon));
        EnemyShooter shooter = new EnemyShooter(model, view.Weapon, weaponStaticData, _projectilePool);

        CannonPresenter presenter = new CannonPresenter(view, model, shooter,
            playerTriggerObserver, _crownsManager);

        view.Behaviour.SetPresenter(presenter);

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

        view.SoundsHandler.Construct(_settingsManager);

        DemonPresenter presenter = new DemonPresenter(model, view, at,
            playerTriggerObserver, _crownsManager);

        view.DemonBehaviour.SetPresenter(presenter);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    public RobotPresenter CreateRobotEnemy(Transform at, PlayerTriggerObserver playerTriggerObserver,
        LevelPresenter level, Transform parent)
    {
        RobotView view = _assets.Instantiate(AssetsPath.RobotPath, at.position)
            .GetComponent<RobotView>();

        WeaponStaticData weaponStaticData = _staticDataService.GetWeaponData(view.Weapon.Type);

        view.transform.SetParent(parent);

        EnemyModel model = new EnemyModel(_staticDataService.ForEnemy(EnemyType.Robot));
        EnemyShooter shooter = new EnemyShooter(model, view.Weapon, weaponStaticData, _projectilePool);

        view.NavMeshAgent.speed = model.MoveSpeed;
        view.NavMeshAgent.angularSpeed = model.RotateSpeed;

        view.SoundsHandler.Construct(_settingsManager);
        // оружие и персонаж используют один обработчик
        //view.Weapon.SoundHandler.Construct(_audioManager);

        RobotPresenter presenter = new RobotPresenter(view, model, at,
            playerTriggerObserver, shooter, _crownsManager);

        view.Behaviour.SetPresenter(presenter);

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
        LevelPresenter level, Transform parent, EnemyType type)
    {
        ZombieView view = _assets.Instantiate(AssetsPath.ZombiePath, spawnPoint.position)
            .GetComponent<ZombieView>();

        view.transform.SetParent(parent);

        EnemyModel model = new EnemyModel(_staticDataService.ForEnemy(type));

        view.NavMeshAgent.speed = model.MoveSpeed;
        view.NavMeshAgent.angularSpeed = model.RotateSpeed;

        view.SoundsHandler.Construct(_settingsManager);

        ZombiePresenter presenter = new ZombiePresenter(view, model, spawnPoint,
            playerTriggerObserver, _crownsManager);

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

        view.SoundsHandler.Construct(_settingsManager);

        GhostPresenter presenter = new GhostPresenter(view, model, spawnPoint,
            playerTriggerObserver, _crownsManager);

        view.GhostBehaviour.SetPresenter(presenter);

        level.Disposables.Add(presenter);
        level.ResetObjects.Add(presenter);

        return presenter;
    }

    private Hud CreateHud(PlayerPresenter playerPresenter)
    {
        var hud = _assets.Instantiate(AssetsPath.HudPath)
            .GetComponent<Hud>();

        hud.Construct(playerPresenter,
            _staticDataService.GetShootAnimationSettings(),
            _levelManager, _uIPoolService,
            _interstitialAdvService);

        foreach (var window in hud.Windows)
            window.Construct(_windowsService);

        foreach (var button in hud.WindowsButton)
            button.Construct(_windowsService);

        foreach (GiftRewardButton giftButton in hud.GiftsRewardButtons)
        {
            GiftsData data = _staticDataService.ForGift(giftButton.Type);

            _timeRewardManager.RegisterGift(giftButton.Type, data.Data.Timer);
            giftButton.Construct(_giftService, _timeRewardManager, data.Data, data.Sprite);
        }

        hud.AudioSettings.Construct(_settingsManager);
        hud.WinsPanel.Construct(_levelManager);
        hud.GiftsButton.Construct(_windowsService, _timeRewardManager);
        hud.UserInfo.Construct(_crownsManager);
        hud.TutorialUI.Construct();

        _uIPoolService.SetCanvas(hud.GetComponent<Canvas>());

        return hud;
    }

    private CameraController CreatePlayerCamera(PlayerView player, UnityEngine.Camera camera)
    {
        return new CameraController(player, camera, _inputService,
            _gameSaveDataService, _settingsManager,
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

    private CannonProjectile CreateCannonProjectile()
    {
        CannonProjectile projectile = _assets.Instantiate(AssetsPath.CannonProjectilePath)
            .GetComponent<CannonProjectile>();

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

    private ShootUIEffect CreateShootUIEffect()
    {
        ShootUIEffect effect = _assets.Instantiate(AssetsPath.ShootUIEffectPath)
            .GetComponent<ShootUIEffect>();

        effect.Construct(_poolService);

        return effect;
    }

    private void CreateBackgroundMusic()
    {
        MusicHandler handler = _assets.Instantiate(AssetsPath.MusicHandlerPath)
            .GetComponent <MusicHandler>();

        handler.Construct(_settingsManager, _staticDataService.GetGameData().BackgroundMusic);
    }
}