using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    [SerializeField] ParticleSystem _particles;

    private IPoolService _pool;

    public void Construct(IPoolService poolService) =>
        _pool = poolService;

    public void Play(Vector3 position, Vector3 normal)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(normal);

        _particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _particles.Play();
    }

    private void OnParticleSystemStopped()
    {
        _particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _pool.Return(this);
    }
}