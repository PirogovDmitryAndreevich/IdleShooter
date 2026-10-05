using UnityEngine;

public interface IRotationHandler
{
    void RotateTowardsMovement(Vector3 direction, Transform transform, float smoothness);
    void RotateTowardsCamera(Transform transform, Camera camera, float smoothness);
    void RotateTowardsPoint(Transform player, Vector3 targetPoint, float smoothness);
}
