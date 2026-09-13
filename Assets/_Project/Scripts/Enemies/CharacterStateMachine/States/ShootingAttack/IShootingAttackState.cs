using UnityEngine;

public interface IShootingAttackState : IPlayerInColliderReaction
{
    Transform AttackTarget { get; }

    void OnEnterAttackState();

    void AttackingUpdate();

    void OnExitAttackState();
}