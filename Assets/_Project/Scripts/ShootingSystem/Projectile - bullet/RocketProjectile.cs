using System.Collections;
using UnityEngine;

public class RocketProjectile : Projectile
{
    [Header("Rocket Projectile Settings")]
    [SerializeField] private float _flySpeed = 5f;
    [SerializeField] private ParticleSystem _flayingParticles;

    private Coroutine _animationCoroutine;
    private IProjectilePool _projectilePool;
    private IPoolService _poolService;

    public void Construct(IProjectilePool projectilePool, IPoolService poolService)
    {
        _projectilePool = projectilePool;
        _poolService = poolService;
    }

    protected override void OnShoot(Vector3 startPosition, Vector3 targetPosition)
    {
        _flayingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _flayingParticles.Play();

        RPGMuzzleEffect muzzle = _poolService.Get<RPGMuzzleEffect>();

        Vector3 direction = (targetPosition - startPosition).normalized;
        muzzle.transform.position = startPosition;
        muzzle.transform.rotation = Quaternion.LookRotation(direction) *
    Quaternion.Euler(0f, -90f, 0f);
        muzzle.Play();

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        _animationCoroutine = StartCoroutine(FlyCoroutine(startPosition, targetPosition));
    }

    protected override void Hit(Vector3 hitPoint, Vector3 hitNormal) => 
        StopFly(hitPoint, hitNormal);

    private IEnumerator FlyCoroutine(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;

        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / _flySpeed;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        StopFly(transform.position, transform.position.normalized);
    }

    private void StopFly(Vector3 hitPoint, Vector3 hitNormal)
    {
        _flayingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Explosion(hitPoint, hitNormal);

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        _projectilePool.Return(ProjectileType.Rocket, this);
    }

    private void Explosion(Vector3 hitPoint, Vector3 hitNormal)
    {
        RocketExplosion explosion = _poolService.Get<RocketExplosion>();

        explosion.Play(hitPoint, hitNormal);
    }
}
