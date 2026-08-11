using UnityEngine;

public class CameraCollision
{
    private readonly Transform _camera;
    private readonly Transform _parent;
    private readonly GameStaticData _gameStaticData;

    private Vector3 _currentPosition;

    public CameraCollision(
        Camera camera,
        GameStaticData gameStaticData,
        Transform parent)
    {
        _camera = camera.transform;
        _parent = parent;
        _gameStaticData = gameStaticData;

        _currentPosition = _camera.position;
    }

    public void Update()
    {
        Vector3 origin = _parent.position;

        Vector3 desiredPosition = _parent.TransformPoint(
            0f,
            _gameStaticData.OffsetDistanceY,
            -_gameStaticData.MaxDistance);

        Vector3 direction = desiredPosition - origin;
        float distance = direction.magnitude;

        if (distance <= 0.001f)
            return;

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
            targetPosition =
                hit.point -
                direction * _gameStaticData.CollisionOffset;
        }

        _currentPosition = Vector3.Lerp(
            _currentPosition,
            targetPosition,
            Time.deltaTime *
            _gameStaticData.CameraCollisionLerpSpeed);

        _camera.position = _currentPosition;
    }
}