using System;

public class PlayerDiedService : IPlayerDiedService
{
    private readonly ITransitionDataService _transitionDataService;    
    private readonly IWindowsService _windowsService;
    private readonly IRewardAdvService _rewardAdvService;
    private PlayerPresenter _player;

    public event Action<LevelType> PlayerDied;

    public PlayerDiedService(ITransitionDataService transitionDataService,
         IWindowsService windowsService, IRewardAdvService rewardAdvService)
    {
        _transitionDataService = transitionDataService;        
        _windowsService = windowsService;
        _rewardAdvService = rewardAdvService;
        _player = _transitionDataService.Player;

        if (_player == null)
            _transitionDataService.PlayerCreated += OnPlayerCreated;
        else
            SetPlayer();

        _rewardAdvService.GiveReward += OnGiveReward;
    }

    public void Dispose()
    {
        _player.PlayerDied -= OnPlayerDied;

        _rewardAdvService.GiveReward -= OnGiveReward;
    }

    public void OnUserShowedAdv() => 
        _rewardAdvService.ShowRewardAdv(RewardType.Revival);

    public void OnUserNoShowedAdv() => 
        PlayerDied?.Invoke(LevelType.Lobby);

    private void OnGiveReward(RewardType type)
    {
        if (_transitionDataService.CurrentLevel.LevelType == LevelType.Level_1)
        {
            PlayerDied?.Invoke(LevelType.Lobby);
            return;
        }

        PlayerDied?.Invoke(_transitionDataService.CurrentLevel.LevelType);
    }

    private void OnPlayerDied()
    {
        if (_transitionDataService.CurrentLevel.LevelType == LevelType.Level_1)
        {
            PlayerDied?.Invoke(LevelType.Lobby);
            return;
        }

        _windowsService.OpenWindow(WindowId.PlayerDied);
    }

    private void OnPlayerCreated()
    {
        _transitionDataService.PlayerCreated -= OnPlayerCreated;

        SetPlayer();
    }

    private void SetPlayer()
    {
        _player = _transitionDataService.Player;
        _player.PlayerDied += OnPlayerDied;
    }
}