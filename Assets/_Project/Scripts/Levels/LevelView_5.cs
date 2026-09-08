using UnityEngine;

public class LevelView_5 : LevelView
{
    [SerializeField] private SphereSpawnPoint[] _sphereSpawnPoints;

    public override LevelType LevelType => LevelType.Level_5;

    public SphereSpawnPoint[] SphereSpawnPoints => _sphereSpawnPoints;
}
