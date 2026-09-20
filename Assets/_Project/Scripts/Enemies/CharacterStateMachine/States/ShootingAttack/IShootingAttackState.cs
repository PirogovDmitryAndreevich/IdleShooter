using UnityEngine;

public interface IShootingAttackState : IPlayerInColliderReaction
{
    Transform AttackTarget { get; }

    void OnEnterShootState();

    void ShootingUpdate();

    void OnExitShootState();
}