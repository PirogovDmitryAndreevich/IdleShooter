using Assets._Project.Scripts.SpawnPoints.Obstacles;

public class LevelView_9 : LevelView
{
    public FlyPlatformSpawnPoint[] FlyPlatformSpawnPoints;
    public SpinningCylinderSpawnPoint[] SpinningCylinderSpawnPoints;
    public GearSpawnPoint[] GearSpawnPoints;
    public GearBaseSpawnPoint[] GearBaseSpawnPoints;

    public override LevelType LevelType =>
        LevelType.Level_9;
}