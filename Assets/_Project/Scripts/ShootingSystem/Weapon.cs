using UnityEngine;

[RequireComponent(typeof(IShooter))]
public class Weapon : MonoBehaviour
{
    [Header("Common Settings")]
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _fireRate = 0.2f;
    
    private float _nextFireTime = 0f;
    private IShooter _shooter;

    public float Damage => _damage; 

    private void Awake()
    {
        _shooter = GetComponent<IShooter>();
    }

    public void TryShoot()
    {
        if(Time.time < _nextFireTime)
            return;

        _nextFireTime = Time.time + _fireRate;
        _shooter.Shoot();
    }
}
