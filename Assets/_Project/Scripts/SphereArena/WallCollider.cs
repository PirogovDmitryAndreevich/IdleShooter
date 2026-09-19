using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WallCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        Vector3 hitNormal = contact.normal;

        MovingSphereObstacle sphere = collision.collider.GetComponent<MovingSphereObstacle>();

        if (sphere == null)
        {
            sphere = collision.collider.GetComponentInChildren<MovingSphereObstacle>();

            if (sphere == null)
                sphere = collision.collider.GetComponentInParent<MovingSphereObstacle>();
        }

        if (sphere != null)        
            sphere.MoveTo(-hitNormal);        
    }
}