using UnityEngine;

public class SphereFinishPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<SphereObstacle>(out SphereObstacle sphere))
            return;

        sphere.ReachFinish();
    }
}