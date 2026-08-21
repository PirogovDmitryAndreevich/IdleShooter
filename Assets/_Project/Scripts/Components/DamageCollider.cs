using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour
{
    private float _damage;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        IPlayerDamageable player = collision.collider.GetComponent<IPlayerDamageable>();

        if (player == null)
        {
            player = collision.collider.GetComponentInChildren<IPlayerDamageable>();

            if (player == null)
                player = collision.collider.GetComponentInParent<IPlayerDamageable>();
        }

        if (player != null)
            player.TakeDamage(_damage, hitPoint, hitNormal);
    }

    public void Construct(float damage) =>
        _damage = damage;
}