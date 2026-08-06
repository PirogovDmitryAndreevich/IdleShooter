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
        UnityEngine.GameObject player = _assets.Instantiate(AssetsPath.CharacterPath, at);

        PlayerModel model = new PlayerModel(_staticDataService, _progressService);

        ThirdPersonController controller = player.GetComponent<ThirdPersonController>();
        controller.Construct(_inputService, model);

        CreatePlayerCamera(player.transform);
    }

    private void CreatePlayerCamera(UnityEngine.Transform player)
    {
        var camera = _assets.Instantiate(AssetsPath.CameraControllerPath)
            .GetComponent<CameraController>();

        camera.Construct(player, _inputService);
    }
}