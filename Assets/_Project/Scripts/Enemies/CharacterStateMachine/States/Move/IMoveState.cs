using UnityEngine;

public interface IMoveState
{
    Transform Target { get; }
    Transform Transform { get; }
    bool IsPlayerInCollider { get; }

    void OnMoveStateEnter();

    void MovingUpdate();

    void OnMoveStateExit();
}