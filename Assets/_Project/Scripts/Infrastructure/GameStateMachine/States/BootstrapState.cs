public class BootstrapState : IState
{
    private const string InitialSceneName = "Initial";

    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly ICoroutineRunner _coroutineRunner;

    public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        ICoroutineRunner coroutineRunner)
    {
        _gameStateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _coroutineRunner = coroutineRunner;

        RegisterService();
    }

    public void Enter() =>
        _sceneLoader.Load(InitialSceneName, onLoaded: EnterLoadLevel);

    public void Exit() { }

    private void EnterLoadLevel()
    {
        _gameStateMachine.Enter<LoadProgressState>();
    }

    private void RegisterService()
    {
        RegisterStaticData();
        AllServices.RegisterSingle<IProgressService>(new ProgressService());
        AllServices.RegisterSingle<ISaveLoadService>(new SaveLoadService(AllServices.Single<IProgressService>()));
        RegisterInputService();

        AllServices.RegisterSingle<IAssets>(new AssetProvider());
        AllServices.RegisterSingle<IProgressService>(new ProgressService());
        AllServices.RegisterSingle<ISaveLoadService>(new SaveLoadService(AllServices.Single<IProgressService>()));
    }

    private void RegisterInputService()
    {
        AllServices.RegisterSingle<IInputService>(new StandaloneInputService());
    }

    private void RegisterStaticData()
    {
        IStaticDataService staticData = new StaticDataService();
        staticData.Load();
        AllServices.RegisterSingle(staticData);
    }
}
