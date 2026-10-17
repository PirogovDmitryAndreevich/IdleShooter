using System;
using UnityEngine;

public interface IPassiveShooting
{
    int Multiplicator { get; }
    Transform PlayerPoint { get; }
    Transform TargetPoint { get; }
}

public class PassiveShooting : MonoBehaviour, IPassiveShooting, IDamageable
{
    [SerializeField] private Transform _playerPoint;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;
    [SerializeField] private int multiplicator = 1;

    protected bool IsAccess = true;
    protected bool IsPlayerInside;
    protected IPlayer Player;

    public Transform PlayerPoint => _playerPoint;
    public Transform TargetPoint => _targetPoint;
    public int Multiplicator => multiplicator;
    public bool IsPlayerInTrigger => IsPlayerInside;

    private void OnDestroy()
    {
        _playerTriggerObserver.PlayerEntered -= OnPlayerEntered;
        _playerTriggerObserver.PlayerExited -= OnPlayerExited;

        Destroy();
    }

    private void Awake()
    {
        _playerTriggerObserver.PlayerEntered += OnPlayerEntered;
        _playerTriggerObserver.PlayerExited += OnPlayerExited;
    }

    public void TakeDamage(float damage)
    {

    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {

    }


    protected virtual void Destroy()
    {

    }

    protected virtual void OnPlayerExited()
    {
        if (!IsAccess)
            return;

        Player.OnStopPassiveShooting();
        Player = null;
        IsPlayerInside = false;
    }

    protected virtual void OnPlayerEntered(IPlayer player)
    {
        Player = player;
        IsPlayerInside = true;

        if (IsAccess)
            Player.PassiveShooting(this);
    }
}
