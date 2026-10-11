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

    public Transform PlayerPoint => _playerPoint;
    public Transform TargetPoint => _targetPoint;
    public int Multiplicator => multiplicator;

    private void OnDestroy() =>
        _playerTriggerObserver.PlayerEntered -= OnPlayerEntered;

    private void Awake() =>
        _playerTriggerObserver.PlayerEntered += OnPlayerEntered;

    private void OnPlayerEntered(IPlayer player) =>
        player.PassiveShooting(this);

    public void TakeDamage(float damage)
    {

    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {

    }
}

public class PassiveShootingReward : MonoBehaviour, IPassiveShooting, IDamageable
{
    [SerializeField] private Transform _playerPoint;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;
    [SerializeField] private int multiplicator = 2;

    public Transform PlayerPoint => _playerPoint;
    public Transform TargetPoint => _targetPoint;
    public int Multiplicator => multiplicator;

    private void OnDestroy() =>
        _playerTriggerObserver.PlayerEntered -= OnPlayerEntered;

    private void Awake() =>
        _playerTriggerObserver.PlayerEntered += OnPlayerEntered;

    private void OnPlayerEntered(IPlayer player) =>
        player.PassiveShooting(this);

    public void TakeDamage(float damage)
    {

    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {

    }
}