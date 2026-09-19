using UnityEngine;

public class MovingSphereObstacle : SphereObstacle
{
    [SerializeField] private float _force;

    private void Start()
    {
        Vector3 direction = UnityEngine.Random.insideUnitSphere;
        direction.y = 0f;

        MoveTo(direction);
    }

    public void MoveTo(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
            return;

        Rigidbody.AddForce(
            direction.normalized * _force,
            ForceMode.Impulse);
    }
}