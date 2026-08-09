public class GameSaveDataService : IGameSaveDataService
{
    private readonly IProgressService _saveData;

    public float Sensitivity =>
        _saveData.Data.Game.Sensitivity;

    public GameSaveDataService(IProgressService progress)
    {
        _saveData = progress;
    }

    public void ChangeSensitivity(float value)
    {
        _saveData.Data.Game.Sensitivity = value;
    }
}