using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;

    public event Action TransitionEntered;

    public LevelType Type { get; private set; }
    public Transform ExitPoint => _exitPoint;
    public Transform PlayerSpawnPoint => _playerSpawnPoint;

    private void OnDestroy() => 
        _playerTriggerObserver.TriggerEnter -= OnPlayerEntered;

    public void Construct(LevelType type)
    {
        Type = type;

        _playerTriggerObserver.TriggerEnter += OnPlayerEntered;
    }

    private void OnPlayerEntered() => 
        TransitionEntered?.Invoke();
}