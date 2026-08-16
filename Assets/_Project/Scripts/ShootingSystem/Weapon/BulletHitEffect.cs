using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class BulletHitEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem _particles;

    private IPoolService _pool;

    public void Construct(IPoolService pool) =>
        _pool = pool;

    public void Play(Vector3 position, Vector3 normal)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(normal);

        _particles.Play();
    }

    private void OnParticleSystemStopped()
    {
        _pool.Return(this);
    }
}
