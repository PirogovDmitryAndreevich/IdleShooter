public interface ISaveLoadService : IService
{
    SaveData LoadProgress();
    void SaveProgress();
}