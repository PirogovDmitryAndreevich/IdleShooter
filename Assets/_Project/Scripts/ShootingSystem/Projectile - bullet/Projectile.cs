using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private float _maxLifetime = 5f;
    private float _damage = 10f;
    private Rigidbody _rigidbody;
    private ProjectilePool _pool;

    private float _timeElapsed = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CountdownLifetime();
    }

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

        if (_pool != null)
            _pool.Return(this);
    }

    public void Startup(float damage, float speed, Vector3 direction, ProjectilePool pool, float maxLifetime)
    {
        _damage = damage;
        _pool = pool;
        _maxLifetime = maxLifetime;
        _timeElapsed = 0f;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void CountdownLifetime()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed >= _maxLifetime)
        {
            if (_pool != null)
                _pool.Return(this);
        }
    }
}
