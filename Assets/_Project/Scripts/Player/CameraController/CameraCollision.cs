using UnityEngine;

public class CameraCollision
{
    private readonly Transform _camera;
    private readonly Transform _parent;
    private readonly GameStaticData _gameStaticData;

    public CameraCollision(
        Camera camera,
        GameStaticData gameStaticData,
        Transform parent)
    {
        _camera = camera.transform;
        _parent = parent;
        _gameStaticData = gameStaticData;
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

        direction.Normalize();

        Vector3 finalPosition = desiredPosition;

        if (Physics.SphereCast(
            origin,
            _gameStaticData.SphereRadius,
            direction,
            out RaycastHit hit,
            distance,
            _gameStaticData.CollisionMask,
            QueryTriggerInteraction.Ignore))
        {
            finalPosition =
                hit.point -
                direction *
                _gameStaticData.CollisionOffset;
        }

        _camera.position = Vector3.Lerp(
            _camera.position,
            finalPosition,
            Time.deltaTime *
            _gameStaticData.CameraCollisionLerpSpeed);
    }
}