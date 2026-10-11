using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;

    public void Play() => 
        _effect.Play();
}