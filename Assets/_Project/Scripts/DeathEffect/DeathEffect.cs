using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathParticle;

    public void Play() => 
        _deathParticle.Play();
}
