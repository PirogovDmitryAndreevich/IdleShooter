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

        _poolService.RegisterFactory(() => CreateBulletHole());
        _poolService.RegisterFactory(() => CreateHitEffect());
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

    public void CreateWeaponSwitcher(WeaponType type, UnityEngine.Vector3 position)
    {
        SwitchWeaponButton switcher = _assets.Instantiate(AssetsPath.WeaponSwitcherPath, at: position)
            .GetComponent<SwitchWeaponButton>();

        switcher.Construct(_weaponSwitcher, _weaponDataService, type);
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

    public Stone CreateStoneProjectile()
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

    {

        muzzle.Construct(_poolService);

        return muzzle;
    }
}