using System.Collections;
using UnityEngine;

public class Minigun : WeaponView
{
    [SerializeField] private ParticleSystem _muzzleEffect;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delayAnimationOff;

    private float _animationDelay;
    private Coroutine _coroutine;

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _animator.SetBool("IsShoot", false);
    }

    public override void Shoot()
    {
        _muzzleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _muzzleEffect.Play();

        _animationDelay = _delayAnimationOff;

        if (_coroutine == null)
        {
            _animator.SetBool("IsShoot", true);
            _coroutine = StartCoroutine(DelayAnimation());
        }
    }

    private IEnumerator DelayAnimation()
    {
        while (true)
        {
            if (_animationDelay <= 0f)
                break;

            _animationDelay -= Time.deltaTime;
            yield return null;
        }

        _animator.SetBool("IsShoot", false);
        _coroutine = null;
    }
}
