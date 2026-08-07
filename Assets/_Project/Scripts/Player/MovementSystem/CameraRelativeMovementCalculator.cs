using UnityEngine;

public class CameraRelativeMovementCalculator : IMovementCalculator
{
    private readonly Camera _camera;
    private PlayerStaticData _data;
    private Vector3 _horizontalMovement;

    public Vector3 HorizontalDirection => _horizontalMovement;

    public CameraRelativeMovementCalculator(Camera camera, PlayerStaticData data)
    {
        _camera = camera;
        _data = data;
    }

    public Vector3 UpdateMoveDirection(Vector2 inputAxis)
    {
        // Оригинальная логика
        float directionX = inputAxis.x * _data.Velocity * Time.deltaTime;
        float directionZ = inputAxis.y * _data.Velocity * Time.deltaTime;

        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Горизонтальное движение
        _horizontalMovement = forward * directionZ + right * directionX;

        return _horizontalMovement;
    }
}