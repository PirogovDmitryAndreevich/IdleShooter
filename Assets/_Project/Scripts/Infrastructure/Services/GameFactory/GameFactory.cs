using System;

public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;
    private readonly IInputService _inputService;
    private readonly IStaticDataService _staticDataService;
    private readonly IProgressService _progressService;
    private readonly IPoolService _poolService;
    private readonly IWeaponDataService _weaponDataService;
    private PlayerPresenter _player;

    public GameFactory(IAssets assets, IInputService inputService, IStaticDataService staticDataService,
        IProgressService progressService, IPoolService poolService, IWeaponDataService weaponDataService)
    {
        _assets = assets;
        _inputService = inputService;
        _staticDataService = staticDataService;
        _progressService = progressService;
        _poolService = poolService;
        _weaponDataService = weaponDataService;

        _poolService.RegisterFactory(() => CreateProjectile());
    }

    public void CreatePlayer(UnityEngine.Vector3 at)
    {
        WeaponSwitcher weaponSwitcher = new WeaponSwitcher(this, _weaponDataService);

        PlayerBehaviour player = _assets.Instantiate(AssetsPath.CharacterPath, at)
            .GetComponent<PlayerBehaviour>();

        player.View.Construct(weaponSwitcher);
        PlayerModel model = new PlayerModel(_staticDataService, _progressService);

        _player = new PlayerPresenter(model, player.View, _inputService, weaponSwitcher);

        player.Initialize(_player);

        CreateHud(weaponSwitcher);
        CreatePlayerCamera(player.View.CameraTarget);
    }

    private void CreateHud(WeaponSwitcher weaponSwitcher)
    {
        var hud = _assets.Instantiate(AssetsPath.HudPath)
            .GetComponent<Hud>();

        hud.Construct(weaponSwitcher, this);
    }

    public WeaponPresenter CreateWeapon(WeaponType type)
    {
        WeaponView view = _assets.Instantiate(_staticDataService.GetWeaponView(type));
        WeaponModel model = new WeaponModel(_staticDataService.GetWeaponData(type));

        WeaponPresenter presenter = new WeaponPresenter(model, view, UnityEngine.Camera.main,
            _staticDataService.GetGameData(), _poolService);

        return presenter;
    }

    public Projectile CreateProjectile()
    {
        var projectile = _assets.Instantiate(AssetsPath.Projectile)
            .GetComponent<Projectile>();

        projectile.Construct(_poolService);

        return projectile;
    }

    public SelectWeaponButton CreateWeaponSelectButton(UnityEngine.Transform parent) => 
        _assets.Instantiate(AssetsPath.SelectWeaponButtonPath, parent)
            .GetComponent<SelectWeaponButton>();

    private void CreatePlayerCamera(UnityEngine.Transform player)
    {
        CameraController camera = _assets.Instantiate(AssetsPath.CameraControllerPath)
            .GetComponent<CameraController>();

        camera.Construct(player, _inputService);
    }
}