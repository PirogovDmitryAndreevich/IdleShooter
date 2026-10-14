public class BootstrapState : IState
{
    private const string InitialSceneName = "Initial";

    private readonly GameStateMachine _gameStateMachine;
    private readonly AllServices _allServices;
    private readonly SceneLoader _sceneLoader;
    private readonly ICoroutineRunner _coroutineRunner;

    public BootstrapState(GameStateMachine gameStateMachine, AllServices allServices, SceneLoader sceneLoader,
        ICoroutineRunner coroutineRunner)
    {
        _gameStateMachine = gameStateMachine;
        _allServices = allServices;
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
        _allServices.RegisterSingle<IProgressService>(new ProgressService());
        _allServices.RegisterSingle<IMetricsService>(new MetricsService(_allServices.Single<IProgressService>()));
        _allServices.RegisterSingle<ISaveLoadService>(new SaveLoadService(_allServices.Single<IProgressService>()));
        _allServices.RegisterSingle<IRewardAdvService>(new RewardAdvService());
        _allServices.RegisterSingle<ITimeRewardManager>(new TimeRewardManager());
        _allServices.RegisterSingle<IPoolService>(new Pool());
        _allServices.RegisterSingle<IUIPoolService>(new UIPoolService());
        _allServices.RegisterSingle<IAssets>(new AssetProvider());
        _allServices.RegisterSingle<IGameSaveDataService>(new GameSaveDataService(_allServices.Single<IProgressService>()));
        _allServices.RegisterSingle<IProjectilePool>(new ProjectilePool());
        _allServices.RegisterSingle<IWarpService>(new WarpService());
        _allServices.RegisterSingle<IInterstitialAdvService>(new InterstitialAdvService());
        _allServices.RegisterSingle<IWindowsService>(new WindowsService());
        _allServices.RegisterSingle<ITransitionDataService>(new TransitionDataService(_allServices.Single<IInterstitialAdvService>()));
        _allServices.RegisterSingle<ISettingsManager>(new SettingsManager(_allServices.Single<IProgressService>()));
        RegisterInputService();

        _allServices.RegisterSingle<IWeaponDataService>(new WeaponDataService(
            _allServices.Single<IProgressService>(),
            _allServices.Single<IMetricsService>()));

        _allServices.RegisterSingle<ILevelManager>(new LevelManager(
            _allServices.Single<IProgressService>(),
            _allServices.Single<IStaticDataService>(),
            _allServices.Single<IMetricsService>()));

        _allServices.RegisterSingle<IWeaponBuyController>(new WeaponBuyController(
            _allServices.Single<ILevelManager>(),
            _allServices.Single<IStaticDataService>(),
            _allServices.Single<IWeaponDataService>()));

        _allServices.RegisterSingle<IGiftService>(new GiftService(_allServices.Single<ILevelManager>()));

        _allServices.RegisterSingle<IGameFactory>(new GameFactory(
            _allServices.Single<IAssets>(),
            _allServices.Single<IInputService>(),
            _allServices.Single<IStaticDataService>(),
            _allServices.Single<IProgressService>(),
            _allServices.Single<IPoolService>(),
            _allServices.Single<IWeaponDataService>(),
            _allServices.Single<IGameSaveDataService>(),
            _allServices.Single<IProjectilePool>(),
            _allServices.Single<IUIPoolService>(),
            _allServices.Single<IWarpService>(),
            _allServices.Single<ITransitionDataService>(),
            _allServices.Single<ILevelManager>(),
            _allServices.Single<IInterstitialAdvService>(),
            _allServices.Single<ISettingsManager>(),
            _allServices.Single<IRewardAdvService>(),
            _allServices.Single<IWindowsService>(),
            _allServices.Single<IGiftService>(),
            _allServices.Single<ITimeRewardManager>(),
            _allServices.Single<IMetricsService>()));

        _allServices.RegisterSingle<ILevelLoader>(new LevelLoader(
            _allServices.Single<IGameFactory>(),
            _coroutineRunner,
            _allServices.Single<ITransitionDataService>()));

        _allServices.RegisterSingle<ILevelController>(new LevelController(
            _allServices.Single<ILevelLoader>(),
            _allServices.Single<IWarpService>(),
            _coroutineRunner));

        _allServices.Single<IGameFactory>()
            .SetLevelController(_allServices.Single<ILevelController>());
    }

    private void RegisterInputService()
    {
        _allServices.RegisterSingle<IInputService>(new StandaloneInputService());
    }

    private void RegisterStaticData()
    {
        IStaticDataService staticData = new StaticDataService();
        staticData.Load();
        _allServices.RegisterSingle(staticData);
    }
}
