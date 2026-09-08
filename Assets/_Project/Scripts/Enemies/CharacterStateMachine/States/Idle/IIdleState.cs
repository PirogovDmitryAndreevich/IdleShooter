public interface IIdleState
{
    bool IsPlayerInCollider { get; }

    void OnIdleStateEnter();
}