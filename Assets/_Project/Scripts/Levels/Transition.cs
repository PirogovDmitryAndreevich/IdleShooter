using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private EnterGate _enterGate;
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;
    [SerializeField] private WinsPoint _winsPoint;

    public event Action TransitionEntered;

    public LevelType Type { get; private set; }
    public Transform ExitPoint => _exitPoint;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    private void OnDestroy() => 
        _playerTriggerObserver.TriggerEnter -= OnPlayerEntered;

    public void Construct(LevelType type, LevelsStaticData data, IWarpService warpService)
    {
        Type = type;

        _enterGate.Construct(data);
        _winsPoint.Construct(data, warpService);

        _playerTriggerObserver.TriggerEnter += OnPlayerEntered;
    }

    private void OnPlayerEntered() => 
        TransitionEntered?.Invoke();
}
