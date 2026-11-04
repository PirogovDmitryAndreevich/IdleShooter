using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private ExitGate _exitGate;
    [SerializeField] private EnterGate _enterGate;
    [SerializeField] private WinsPoint _winsPoint;

    public event Action TransitionEntered;
    public event Action PlayerReturnedBack;
    public event Action PlayerGiveWins;

    private bool _isPlayerEntered;

    public LevelType Type { get; private set; }

    private IMetricsService _metrics;

    public Transform ExitPoint => _exitPoint;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;
    public Transform WinsPoint => _winsPoint.transform;

    private void OnDestroy()
    {
        if (_enterGate != null)
            _enterGate.PlayerEntered -= OnPlayerReturnedBack;

        if (_exitGate != null)
            _exitGate.PlayerEntered -= OnPlayerExitNext;

        if (_winsPoint != null)
            _winsPoint.PlayerGiveWins -= OnPlayerGiveWins;
    }

    public void Construct(LevelType type, LevelsStaticData data,
        ILevelController levelController, ILevelManager levelManager,
        IMetricsService metrics)
    {
        Type = type;

        _metrics = metrics;

        if (_exitGate != null)
        {
            _exitGate.Construct(data);
            _exitGate.PlayerEntered += OnPlayerExitNext;
        }

        if (_enterGate != null)
            _enterGate.PlayerEntered += OnPlayerReturnedBack;


        if (type == LevelType.Level_1
            || type == LevelType.Lobby)
            return;

        _winsPoint.Construct(data, levelController, levelManager);

        _winsPoint.PlayerGiveWins += OnPlayerGiveWins;
    }

    public void OnReset() =>        
        _isPlayerEntered = false;

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

    private void OnPlayerExitNext()
    {
        if (!_isPlayerEntered)
        {
            OnPlayerEntered();
            return;
        }

        _isPlayerEntered = false;
    }

    private void OnPlayerReturnedBack()
    {
        if (!_isPlayerEntered)
        {
            OnPlayerEntered();
            return;
        }

        _isPlayerEntered = false;
        PlayerReturnedBack?.Invoke();
    }

    private void OnPlayerEntered()
    {
        UnityEngine.Debug.Log("Player Entered in Transition " + Type);

        _isPlayerEntered = true;
        _metrics.ReachedGameLevel(Type);
        TransitionEntered?.Invoke();
    }

    private void OnPlayerGiveWins()
    {
        _isPlayerEntered = false;
        PlayerGiveWins?.Invoke();
    }
}
