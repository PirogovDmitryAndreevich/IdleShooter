public class GameFactory : IGameFactory
{
    private readonly IAssets _assets;
    private readonly IInputService _inputService;
    private readonly IStaticDataService _staticDataService;
    private readonly IProgressService _progressService;

    public GameFactory(IAssets assets, IInputService inputService, IStaticDataService staticDataService,
        IProgressService progressService)
    {
        _assets = assets;
        _inputService = inputService;
        _staticDataService = staticDataService;
        _progressService = progressService;
    }

    public void CreatePlayer(UnityEngine.Vector3 at)
    {
        PlayerBehaviour player = _assets.Instantiate(AssetsPath.CharacterPath, at)
            .GetComponent<PlayerBehaviour>();

        PlayerModel model = new PlayerModel(_staticDataService, _progressService);
        PlayerPresenter presenter = new PlayerPresenter(model, player.View, _inputService);

        player.Initialize(presenter);

        CreatePlayerCamera(player.View.CameraTarget);
    }

    private void CreatePlayerCamera(UnityEngine.Transform player)
    {
        var camera = _assets.Instantiate(AssetsPath.CameraControllerPath)
            .GetComponent<CameraController>();

        camera.Construct(player, _inputService);
    }
}