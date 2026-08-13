using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private TrailRenderer _trail;

    private IProjectilePool _pool;
    private ProjectileType _type;
    private float _damage;

    private void OnCollisionEnter(Collision collision)
    {
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

        Die();
    }

    public void Construct(IProjectilePool poolService, ProjectileType type)
    {
        _pool = poolService;
        _type = type;
    }

    public void Shoot(float damage, Vector3 targetPosition)
    {
        _damage = damage;

        _trail.Clear();
        _trail.enabled = true;

        StartFly(targetPosition);
    }

    protected abstract void StartFly(Vector3 targetPosition);    

    protected virtual void Die()
    {  
        _trail.Clear();
        _trail.enabled = false;

        _pool.Return(_type, this);
    }
}
