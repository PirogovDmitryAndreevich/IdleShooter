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
    private PlayerPresenter _player;
    WeaponSwitcher _weaponSwitcher;

    public GameFactory(IAssets assets, IInputService inputService, IStaticDataService staticDataService,
        IProgressService progressService, IPoolService poolService, IWeaponDataService weaponDataService,
        IGameSaveDataService gameSaveDataService, IProjectilePool projectilePool)
    {
        _assets = assets;
        _inputService = inputService;
        _staticDataService = staticDataService;
        _progressService = progressService;
        _poolService = poolService;
        _weaponDataService = weaponDataService;
        _gameSaveDataService = gameSaveDataService;
        _projectilePool = projectilePool;

        _projectilePool.Register(ProjectileType.Stone, () => CreateStoneProjectile());
        _projectilePool.Register(ProjectileType.Minigun, () => CreateMinigunProjectile());
        _projectilePool.Register(ProjectileType.Rocket, () => CreateRocketProjectile());

        _poolService.RegisterFactory(() => CreateBulletHole());
        _poolService.RegisterFactory(() => CreateHitEffect());
        _poolService.RegisterFactory(() => CreateRPGMuzzle());
        _poolService.RegisterFactory(() => CreateRocketExplosion());
    }

    public void CreatePlayer(UnityEngine.Vector3 at)
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
    }

    public Transition CreateTransition(Transform at)
    {
        var transition = _assets.Instantiate(AssetsPath.TransitionPath, at: at.position)
            .GetComponent<Transition>();

        transition.transform.rotation = at.rotation;

        return transition;
    }

    public Lobby CreateLobby()
    {
        GameStaticData gameData = _staticDataService.GetGameData();

        Lobby lobby = _assets.Instantiate(
            _staticDataService.GetLevelView<Lobby>(LevelType.Lobby),
            at: gameData.LobbySpawnPosition);

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

    public void CreateWeaponSwitcher(WeaponType type, UnityEngine.Vector3 position)
    {
        SwitchWeaponButton switcher = _assets.Instantiate(AssetsPath.WeaponSwitcherPath, at: position)
            .GetComponent<SwitchWeaponButton>();

        WeaponStaticData weaponData = _staticDataService.GetWeaponData(type);

        switcher.Construct(_weaponSwitcher, _weaponDataService, type, weaponData);
    }

    public Target CreateTarget(UnityEngine.Vector3 position)
    {
        Target target = _assets.Instantiate(AssetsPath.TargetPath, at: position)
            .GetComponent<Target>();

        return target;
    }

    public SpinningCylinder CreateSpinningCylinder(UnityEngine.Vector3 position)
    {
        SpinningCylinder cylinder = _assets.Instantiate(AssetsPath.SpinningCylinderPath, at: position)
            .GetComponent<SpinningCylinder>();

        return cylinder;
    }

    public Gear CreateGear(UnityEngine.Vector3 position)
    {
        Gear gear = _assets.Instantiate(AssetsPath.GearPath, at: position)
            .GetComponent<Gear>();

        return gear;
    }

    public GearBase CreateGearBase(UnityEngine.Vector3 position)
    {
        GearBase gear = _assets.Instantiate(AssetsPath.GearBasePath, at: position)
            .GetComponent<GearBase>();

        return gear;
    }

    public SpikeCylinder CreateSpikeCylinder(UnityEngine.Vector3 position)
    {
        SpikeCylinder gear = _assets.Instantiate(AssetsPath.SpikeCylinderPath, at: position)
            .GetComponent<SpikeCylinder>();

        return gear;
    }

    private Hud CreateHud(PlayerPresenter playerPresenter)
    {
        var hud = _assets.Instantiate(AssetsPath.HudPath)
            .GetComponent<Hud>();

        hud.Construct(playerPresenter);

        return hud;
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
            .GetComponent <RPGMuzzleEffect>();

        muzzle.Construct(_poolService);

        return muzzle;
    }
}