public class GameSaveDataService : IGameSaveDataService
{
    private readonly IProgressService _saveData;
    private readonly ISaveLoadService _save;

    public float Sensitivity =>
        _saveData.Data.Game.Sensitivity;

    public GameSaveDataService(IProgressService progress, ISaveLoadService save)
    {
        _saveData = progress;
        _save = save;
    }

    public void ChangeSensitivity(float value)
    {
        _saveData.Data.Game.Sensitivity = value;
        _save.SaveProgress();
    }
}