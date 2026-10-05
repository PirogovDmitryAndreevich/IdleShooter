using UnityEngine;

public class PassiveShooting : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _playerPoint;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private PlayerTriggerObserver _playerTriggerObserver;
    [SerializeField] private int multiplicator = 1;

    public Transform PlayerPoint => _playerPoint;
    public Transform TargetPoint => _targetPoint;
    public int Multiplicator => multiplicator;

    private void OnDestroy() => 
        _playerTriggerObserver.PassiveShooterEntered -= OnPlayerEntered;

    private void Awake() => 
        _playerTriggerObserver.PassiveShooterEntered += OnPlayerEntered;

    private void OnPlayerEntered(IPassiveShooterInteractor passiveShooter) => 
        passiveShooter.PassiveShooting(this);

    public void TakeDamage(float damage)
    {
        
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        
    }
}