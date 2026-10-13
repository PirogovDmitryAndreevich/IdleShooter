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
    public Transform ExitPoint => _exitPoint;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    private void OnDestroy()
    {
        _playerTriggerObserver.TriggerEnter -= OnPlayerEntered;

        if (_winsPoint != null)
            _winsPoint.PlayerGiveWins -= OnPlayerGiveWins;
    }

    public void Construct(LevelType type, LevelsStaticData data,
        ILevelController levelController, ILevelManager levelManager)
    {
        Type = type;

        _playerTriggerObserver.TriggerEnter += OnPlayerEntered;

        if (type == LevelType.Level_1
            || type == LevelType.Lobby)
            return;

        _exitGate.Construct(data);
        _winsPoint.Construct(data, levelController, levelManager);

        _winsPoint.PlayerGiveWins += OnPlayerGiveWins;
    }

    public void SetState(bool isOpen)
    {
        if (Type == LevelType.Level_1
            || Type == LevelType.Lobby)
            return;

        if (isOpen)
            _enterGate.Open();
        else
            _enterGate.Lock();
    }

    private void OnPlayerEntered() =>
        TransitionEntered?.Invoke();

    private void OnPlayerGiveWins() =>
        PlayerGiveWins?.Invoke();
}
