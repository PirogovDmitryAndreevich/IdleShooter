using UnityEngine;

public interface IMoveState : IPlayerInColliderReaction
{
    Transform MoveTarget { get; }
    Transform Transform { get; }

    void OnMoveStateEnter();

    void MovingUpdate();

    void OnMoveStateExit();
}