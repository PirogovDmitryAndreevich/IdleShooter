using UnityEngine;

public class Ak : WeaponView
{
    [SerializeField] private ParticleSystem _muzzleEffect;

    public override void Shoot()
    {
        _muzzleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _muzzleEffect.Play();
    }
}
