using UnityEngine;

public class LevelView_6 : LevelView
{
    [SerializeField] private ZombieSpawnPoint[] _spawnPoints;
    [SerializeField] private PlayerTriggerObserver _triggerObserver;

    public override LevelType LevelType => LevelType.Level_6;
    public ZombieSpawnPoint[] ZombiesSpawnPoints => _spawnPoints;
    public PlayerTriggerObserver PlayerTriggerObserver =>
        _triggerObserver;
}