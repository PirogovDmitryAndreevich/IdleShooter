using UnityEngine;

public class LevelView_7 : LevelView
{
    public GhostSpawnPoint GhostSpawnPoint;

    [SerializeField] private PlayerTriggerObserver _triggerObserver;

    public override LevelType LevelType => LevelType.Level_7;
    public PlayerTriggerObserver PlayerTriggerObserver =>
        _triggerObserver;
}
