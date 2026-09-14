using UnityEngine;

public class Cannon : WeaponView
{
    [SerializeField] private ParticleSystem _muzzleEffect;

    public override void Shoot()
    {
        _muzzleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _muzzleEffect.Play();
    }
}