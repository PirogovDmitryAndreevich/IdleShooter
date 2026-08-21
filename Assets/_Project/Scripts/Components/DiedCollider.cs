using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DiedCollider : MonoBehaviour
{
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
            player.Died();
    }
}