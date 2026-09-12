using UnityEngine;

public class LevelView_6 : LevelView
{
    public ZombieSpawnPoint[] ZombiesSpawnPoints;

    [SerializeField] private PlayerTriggerObserver _triggerObserver;

    public override LevelType LevelType => LevelType.Level_6;
    public PlayerTriggerObserver PlayerTriggerObserver =>
        _triggerObserver;
}
