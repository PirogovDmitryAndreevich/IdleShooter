public interface IStaticDataService : IService
{
    public void Load();

    PlayerStaticData ForPlayer();
}