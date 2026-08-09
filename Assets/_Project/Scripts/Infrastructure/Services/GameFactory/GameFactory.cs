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

        CreatePlayerCamera(player.View.CameraTarget);
    }

    private void CreatePlayerCamera(UnityEngine.Transform player)
    {
        CameraController camera = _assets.Instantiate(AssetsPath.CameraControllerPath)
            .GetComponent<CameraController>();

        camera.Construct(player, _inputService);
    }
}