using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private Collider _collider;
    [SerializeField] private GameObject _mesh;
    [SerializeField] private HitParticles _hitParticles;

    private IProjectilePool _projectilePool;
    private IPoolService _poolService;
    private ProjectileType _type;
    private float _damage;

    private bool _isFinished;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isFinished)
            return;

        _isFinished = true;

        ContactPoint contact = collision.contacts[0];

        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        BaseDamageable damageable = collision.collider.GetComponent<BaseDamageable>();

        if (damageable == null)
        {
            damageable = collision.collider.GetComponentInParent<BaseDamageable>();

            if (damageable == null)
                damageable = collision.collider.GetComponentInChildren<BaseDamageable>();
        }

        if (damageable != null)        
            damageable.TakeDamage(_damage, hitPoint, hitNormal);

        _mesh.SetActive(false);
        _trail.Clear();
        _trail.enabled = false;
        _collider.enabled = false;

        _hitParticles.Play(
        contact.point,
        contact.normal);

        var bulletHole = _poolService.Get<BulletHole>();
        bulletHole.Show(hitPoint, hitNormal);
    }

    public void Construct(IProjectilePool projectilePool, ProjectileType type, IPoolService poolService)
    {
        _projectilePool = projectilePool;
        _poolService = poolService;
        _type = type;

        _hitParticles.Construct(this);
    }

    public void Shoot(float damage,Vector3 startPosition, Vector3 targetPosition)
    {
        _damage = damage;
        _isFinished = false;

        transform.position = startPosition;

        _mesh.SetActive(true);

        _trail.Clear();
        _trail.enabled = true;

        _collider.enabled = true;

        StartFly(startPosition, targetPosition);
    }

    public void ReturnToPool()
    {
        if (!_isFinished)
            _isFinished = true;

        StopFly();
        Hide();

        _projectilePool.Return(_type, this);
    }

    protected abstract void StartFly(Vector3 startPosition, Vector3 targetPosition);

    protected virtual void StopFly()
    {
    }

    protected void FinishFlight()
    {
        if (_isFinished)
            return;

        _isFinished = true;

        StopFly();
        Hide();
        
        _projectilePool.Return(_type, this);
    }

    private void Hide()
    {
        _mesh.SetActive(false);

        _trail.Clear();
        _trail.enabled = false;

        _collider.enabled = false;
    }
}
