using YG;

public class SaveLoadService : ISaveLoadService
{
    private readonly IProgressService _progressService;

    public SaveLoadService(IProgressService progressService) =>
        _progressService = progressService;

    public void SaveProgress() =>
        YandexGamesSave();

    public SaveData LoadProgress()
    {
        return YG2.saves.Data;
    }

    private void YandexGamesSave()
    {
        YG2.saves.Data = _progressService.Data;
        YG2.SaveProgress();
    }
}