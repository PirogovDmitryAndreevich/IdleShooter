using Assets._Project.Scripts.SpawnPoints.Obstacles;

public class LevelView_2 : LevelView
{
    public override LevelType LevelType => LevelType.Level_2;

    public TargetSpawnPoint[] TargetsSpawnPoints;
    public SpinningCylinderSpawnPoint[] SpinningCylinderSpawnPoint;
    public SpikeCylinderSpawnPoint[] SpikeCylinderSpawnPoints;
    public FlyPlatformSpawnPoint FlyPlatformSpawnPoint;
    public GearSpawnPoint GearSpawnPoint;
    public GearBaseSpawnPoint GearBaseSpawnPoint;
}
