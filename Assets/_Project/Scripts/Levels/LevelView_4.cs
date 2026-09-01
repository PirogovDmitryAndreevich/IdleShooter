using UnityEngine;

public class LevelView_4 : LevelView
{
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;

    public override LevelType LevelType => LevelType.Level_4;

    public DemonSpawnPoint DemonSpawnPoint;

    public PlayerTriggerObserver PlayerTriggerObserver =>
         _playerTriggerObserver;
}