using UnityEngine;

public class CameraCollision
{
    private readonly Transform _parent;
    private readonly GameStaticData _gameStaticData;

    public CameraCollision(
        GameStaticData gameStaticData,
        Transform parent)
    {
        _parent = parent;
        _gameStaticData = gameStaticData;
    }

    public Vector3 CalculatePosition()
    {
        Vector3 origin = _parent.position;

        Vector3 desiredPosition = _parent.TransformPoint(
            0f,
            _gameStaticData.OffsetDistanceY,
            -_gameStaticData.MaxDistance);

        Vector3 direction = desiredPosition - origin;
        float distance = direction.magnitude;

        if (distance <= 0.001f)
            return desiredPosition;

        direction.Normalize();

        Vector3 targetPosition = desiredPosition;

        if (Physics.SphereCast(
            origin,
            _gameStaticData.SphereRadius,
            direction,
            out RaycastHit hit,
            distance,
            _gameStaticData.CollisionMask,
            QueryTriggerInteraction.Ignore))
        {
            return hit.point -
                  direction * _gameStaticData.CollisionOffset;
        }

        return desiredPosition;
    }
}