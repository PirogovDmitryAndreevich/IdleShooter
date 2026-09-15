using UnityEngine;

public class LevelView_8 : LevelView
{
    public CannonSpawnPoint[] CannonSpawnPoints;

    [SerializeField] private PlayerTriggerObserver _triggerObserver;

    public override LevelType LevelType => LevelType.Level_8;
    public PlayerTriggerObserver PlayerTriggerObserver =>
        _triggerObserver;
}
