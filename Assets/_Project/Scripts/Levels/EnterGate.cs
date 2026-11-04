using System;
using UnityEngine;
using UnityEngine.UI;

public class EnterGate : MonoBehaviour
{
    [SerializeField] private Image _lock;
    [SerializeField] private Collider _collider;
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;

    public event Action PlayerEntered;

    private void OnDestroy() => 
        _playerTriggerObserver.TriggerEnter -= OnPlayerEntered;

    private void Start() => 
        Lock();

    public void Lock()
    {
        if (_lock == null || _collider == null)
            return;

        _lock.gameObject.SetActive(true);
        _collider.isTrigger = false;

        _playerTriggerObserver.TriggerEnter += OnPlayerEntered;
    }

    private void OnPlayerEntered() => 
        PlayerEntered?.Invoke();

    public void Open()
    {
        if (_lock == null || _collider == null)
            return;

        _lock.gameObject.SetActive(false);
        _collider.isTrigger = true;
    }
}