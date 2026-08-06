public class PlayerModel
{
    private readonly PlayerStaticData _playerStaticData;
    private readonly PlayerSaveData _playerSaveData;

    public PlayerStaticData StaticData => _playerStaticData;
    public PlayerSaveData SaveData => _playerSaveData;

    public PlayerModel(IStaticDataService staticData, IProgressService progressService)
    {
        _playerStaticData = staticData.ForPlayer();
        _playerSaveData = progressService.Data.PlayerData;
    }
}