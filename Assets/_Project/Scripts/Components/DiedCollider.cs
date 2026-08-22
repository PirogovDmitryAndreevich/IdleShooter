using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DiedCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        IPlayer player = collision.collider.GetComponent<IPlayer>();

        if (player == null)
        {
            player = collision.collider.GetComponentInChildren<IPlayer>();

            if (player == null)
                player = collision.collider.GetComponentInParent<IPlayer>();
        }

        if (player != null)
            player.Died();
    }
}