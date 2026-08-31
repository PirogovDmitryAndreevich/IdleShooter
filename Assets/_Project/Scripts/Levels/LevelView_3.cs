using UnityEngine;

public class LevelView_3 : LevelView
{
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;

    public override LevelType LevelType => LevelType.Level_3;

    public SoldierSpawnPoint[] SoldierSpawnPoints;

    public PlayerTriggerObserver PlayerTriggerObserver =>
         _playerTriggerObserver;
}