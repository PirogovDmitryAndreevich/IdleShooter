public class LevelView_2 : LevelView
{
    public override LevelType LevelType => LevelType.Level_2;

    public ObstacleSpawnPoint[] ObstacleSpawnPoints;
    public ObstacleSpawnPoint GearForPlatformMoving;
    public FlyPlatformSpawnPoint FlyPlatformSpawnPoint;
}
