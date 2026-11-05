using UnityEngine;

public class CameraRelativeMovementCalculator : IMovementCalculator
{
    private readonly Camera _camera;
    private Vector3 _horizontalMovement;

    public Vector3 HorizontalDirection => _horizontalMovement;

    public CameraRelativeMovementCalculator(Camera camera) => 
        _camera = camera;

    public Vector3 UpdateMoveDirection(Vector2 inputAxis, float speed)
    {
        // Оригинальная логика
        float directionX = inputAxis.x * speed * Time.deltaTime;
        float directionZ = inputAxis.y * speed * Time.deltaTime;

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