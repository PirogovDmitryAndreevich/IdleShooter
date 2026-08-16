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

        BaseDamageable damageable = collision.collider.GetComponent<BaseDamageable>();

        if (damageable == null)
        {
            damageable = collision.collider.GetComponentInParent<BaseDamageable>();

            if (damageable == null)
                damageable = collision.collider.GetComponentInChildren<BaseDamageable>();
        }

        if (damageable != null)
            damageable.TakeDamage(_damage, hitPoint, hitNormal);

        Hit(hitPoint, hitNormal);
    }

    public void Shoot(float damage, Vector3 startPosition, Vector3 targetPosition)
    {
        _damage = damage;
        _isCollision = false;
        _collider.enabled = true;

        OnShoot(startPosition, targetPosition);
    }

    protected abstract void OnShoot(Vector3 startPosition, Vector3 targetPosition);

    protected abstract void Hit(Vector3 hitPoint, Vector3 hitNormal);
}