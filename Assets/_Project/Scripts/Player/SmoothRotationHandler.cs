using UnityEngine;

public class SmoothRotationHandler : IRotationHandler
{
    public void RotateTowardsMovement(Vector3 direction, Transform transform, float smoothness)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            smoothness
        );
    }

    public void RotateTowardsCamera(Transform transform, Camera camera, float smoothness)
    {
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        if (cameraForward == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            smoothness
        );
    }

    public void RotateTowardsPoint(Transform player, Vector3 targetPoint, float smoothness)
    {
        Vector3 direction = targetPoint - player.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        player.rotation = Quaternion.Slerp(
            player.rotation,
            targetRotation,
            smoothness * Time.deltaTime);
    }
}