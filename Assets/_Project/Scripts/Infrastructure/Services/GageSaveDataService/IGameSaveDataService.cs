public interface IGameSaveDataService : IService
{
    float Sensitivity { get; }

    void ChangeSensitivity(float value);
}