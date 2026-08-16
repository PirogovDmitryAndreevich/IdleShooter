using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RPGMuzzleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;

    private IPoolService _pool;

    public void Construct(IPoolService pool) =>
        _pool = pool;

    public void Play() => 
        _particles.Play();

    private void OnParticleSystemStopped() => 
        _pool.Return(this);
}