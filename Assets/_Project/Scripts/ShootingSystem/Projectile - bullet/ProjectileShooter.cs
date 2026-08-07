using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class ProjectileShooter : MonoBehaviour, IShooter
{
    [Header("Reference Settings")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private ProjectilePool _projectilePool;

    [Header("Projectile Settings")]
    [SerializeField] private float _projectileSpeed = 30f;
    [SerializeField] private float _maxAimDistance = 1000f;
    [SerializeField] private LayerMask _aimMask;

    private Weapon _weapon;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();

        if(_playerCamera == null)
            _playerCamera = Camera.main;
    }

    public void Shoot()
    {
        Vector3 aimPoint = GetAimPointFromCamera();
        Vector3 direction = (aimPoint - _shootPoint.position).normalized;

        Projectile projectile = _projectilePool.Get();
        projectile.transform.position = _shootPoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(direction);

        projectile.Startup(
            _weapon.Damage,
            _projectileSpeed,
            direction,
            _projectilePool,
            _projectilePool.MaxLifetime);
    }    

    private Vector3 GetAimPointFromCamera()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if(Physics.Raycast(ray, out RaycastHit hit, _maxAimDistance, _aimMask))
        {
            return hit.point;
        }

        return ray.origin + ray.direction * _maxAimDistance;
    }
}
