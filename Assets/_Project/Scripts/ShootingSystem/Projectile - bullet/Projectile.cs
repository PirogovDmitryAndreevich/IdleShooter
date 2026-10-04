using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private Collider _collider;

    private float _damage;

    private bool _isCollision;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollision)
            return;

        _isCollision = true;
        _collider.enabled = false;

        ContactPoint contact = collision.contacts[0];

        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        IDamageable damageable = collision.collider.GetComponent<IDamageable>();

        if (damageable == null)
        {
            damageable = collision.collider.GetComponentInParent<IDamageable>();

            if (damageable == null)
                damageable = collision.collider.GetComponentInChildren<IDamageable>();
        }

        bool isDamageable = damageable != null;

        if (isDamageable)
            damageable.TakeDamage(_damage, hitPoint, hitNormal);

            Hit(hitPoint, hitNormal, isDamageable);
    }

    public void Shoot(float damage, Vector3 startPosition, Vector3 targetPosition)
    {
        _damage = damage;
        _isCollision = false;
        _collider.enabled = true;

        OnShoot(startPosition, targetPosition);
    }

    protected abstract void OnShoot(Vector3 startPosition, Vector3 targetPosition);

    protected abstract void Hit(Vector3 hitPoint, Vector3 hitNormal, bool isDamageable);
}