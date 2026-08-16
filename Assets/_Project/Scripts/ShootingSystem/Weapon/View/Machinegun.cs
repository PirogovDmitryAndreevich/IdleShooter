using UnityEngine;

public class Machinegun : WeaponView
{
    [SerializeField] private ParticleSystem _muzzleEffect;

    public override void Shoot()
    {
        _muzzleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _muzzleEffect.Play();
    }
}
