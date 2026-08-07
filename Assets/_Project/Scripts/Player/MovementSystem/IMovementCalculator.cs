using UnityEngine;

public interface IMovementCalculator
{
    Vector3 HorizontalDirection { get; }

    Vector3 UpdateMoveDirection(Vector2 inputAxis);
}