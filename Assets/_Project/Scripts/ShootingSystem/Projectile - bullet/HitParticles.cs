using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem _particles;

    private Projectile _projectile;

    public void Construct(Projectile projectile) => 
        _projectile = projectile;

    public void Play(Vector3 position, Vector3 normal)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(normal);

        _particles.Play();
    }

    private void OnParticleSystemStopped()
    {
        _projectile.ReturnToPool();
    }
}