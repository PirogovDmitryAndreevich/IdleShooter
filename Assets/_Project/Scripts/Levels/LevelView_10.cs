public class LevelView_10 : LevelView
{
    public CannonSpawnPoint[] CannonsSpawnPoints;
    public FlyPlatformAndGear[] PlatformsAndGears;
    public SoldierSpawnPoint[] SoldierSpawnPoints;
    public ZombieSpawnPoint[] ZombieSpawnPoints;
    public FlyPlatformSpawnPoint[] FlyPlatformSpawnPoints;
    public PlayerTriggerObserver PlayerTriggerObserver;
    public SphereSpawnPoint[] SphereSpawnPoints;

    public override LevelType LevelType =>
        LevelType.Level_10;
}