using UnityEngine;

public class Gun : WeaponView
{
    [SerializeField] private ParticleSystem _muzzleEffect;

    public override void Shoot()
    {
        _muzzleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _muzzleEffect.Play();
    }
}
