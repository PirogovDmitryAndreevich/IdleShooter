using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MuzzleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;

    private IPoolService _pool;

    public void Construct(IPoolService pool)
    {
        _pool = pool;
    }

    public void Play(Transform shootPoint, Vector3 direction)
    {
        transform.position = shootPoint.position;
        transform.rotation = Quaternion.LookRotation(direction);

        _particle.Play();
    }

    private void OnParticleSystemStopped()
    {
        _pool.Return(this);
    }
}