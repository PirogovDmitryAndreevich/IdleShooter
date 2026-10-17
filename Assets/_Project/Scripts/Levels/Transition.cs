using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private ExitGate _exitGate;
    [SerializeField] private EnterGate _enterGate;
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;
    [SerializeField] private WinsPoint _winsPoint;

    public event Action TransitionEntered;
    public event Action PlayerGiveWins;

    public LevelType Type { get; private set; }

    private IMetricsService _metrics;

    public Transform ExitPoint => _exitPoint;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;
    public Transform WinsPoint => _winsPoint.transform;

    private void OnDestroy()
    {
        _playerTriggerObserver.TriggerEnter -= OnPlayerEntered;

        if (_winsPoint != null)
            _winsPoint.PlayerGiveWins -= OnPlayerGiveWins;
    }

    public void Construct(LevelType type, LevelsStaticData data,
        ILevelController levelController, ILevelManager levelManager,
        IMetricsService metrics)
    {
        Type = type;

        _metrics = metrics;

        _playerTriggerObserver.TriggerEnter += OnPlayerEntered;

        if (_exitGate != null)
            _exitGate.Construct(data);

        if (type == LevelType.Level_1
            || type == LevelType.Lobby)
            return;

        _winsPoint.Construct(data, levelController, levelManager);

        _winsPoint.PlayerGiveWins += OnPlayerGiveWins;
    }

    public void SetState(bool isOpen)
    {
        if (_enterGate == null)
            return;

        if (Type == LevelType.Level_1
            || Type == LevelType.Lobby)
            return;

        if (isOpen)
            _enterGate.Open();
        else
            _enterGate.Lock();
    }

    private void OnPlayerEntered()
    {
        TransitionEntered?.Invoke();
        _metrics.ReachedGameLevel(Type);
    }

    private void OnPlayerGiveWins() =>
        PlayerGiveWins?.Invoke();
}
