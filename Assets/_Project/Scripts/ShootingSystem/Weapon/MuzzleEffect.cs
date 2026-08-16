using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MuzzleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;

    public void Play()
    {
        _particle.Play();
    }

    private void OnParticleSystemStopped()
    {
        _particle.Stop();
    }
}