using UnityEngine;

public class ObstacleView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private HitFlash _hitEffect;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private DamageCollider _damageCollider;
    [SerializeField] private SoundsHandler _soundsHandler;
    [SerializeField] private GameObject _mesh;
    [SerializeField] private DestroyEffect _destroyEffect;

    public HealthBar HealthBar =>
        _healthBar;
    public DamageableCollider DamageableCollider =>
        _damageableCollider;
    public HitFlash HitFlash =>
        _hitEffect;
    public DamageCollider DamageCollider =>
        _damageCollider;
    public SoundsHandler SoundsHandler =>
        _soundsHandler;
    public GameObject Mesh => _mesh;

    public void OnDied()
    {
        _mesh.SetActive(false);
        _destroyEffect.Play();
    }

    public void OnReset()
    {
        _mesh.SetActive(true);
    }
}
