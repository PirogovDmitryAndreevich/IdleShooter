public interface ITakingDamage
{
    bool IsTakingDamageCompleted { get; }
    bool IsTakedDamage { get; }

    void OnTakeDamageStateEnter();
    void OnTakeDamageStateUpdate();
    void OnTakeDamageStateExit();
}