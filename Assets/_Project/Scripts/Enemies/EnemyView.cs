using UnityEngine;
using UnityEngine.AI;

public class EnemyView : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private HitFlash _hitEffect;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private HealthBar _healthBar;

    public HitFlash HitEffect => _hitEffect;

    public NavMeshAgent NavMeshAgent =>
        _navMeshAgent;

    public DamageableCollider DamageableCollider =>
        _damageableCollider;

    public HealthBar HealthBar =>
        _healthBar;
}