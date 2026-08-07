using UnityEngine;

public interface IRotationHandler
{
    void RotateTowardsMovement(Vector3 direction, Transform transform, float smoothness);
    void RotateTowardsCamera(Transform transform, Camera camera, float smoothness);
}
