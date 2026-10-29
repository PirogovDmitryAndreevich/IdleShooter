using System.Collections;
using UnityEngine;

public class CannonProjectile : Projectile
{
    [Header("Cannon Projectile Settings")]
    [SerializeField] private float _flySpeed = 5f;
    [SerializeField] private float _flyHeight = 1f;
    [SerializeField] AnimationCurve _flyCurve;
    [SerializeField] private TrailRenderer _trail;

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
        transform.position = startPosition;

        _trail.Clear();
        _trail.enabled = true;

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        _animationCoroutine = StartCoroutine(FlyCoroutine(startPosition, targetPosition));
    }

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

            transform.position = Vector3.Lerp(startPosition, targetPosition, t)
                + _flyCurve.Evaluate(t) * _flyHeight * Vector3.up;
            yield return null;
        }

        StopFly(transform.position, transform.position.normalized);
    }

    protected override void Hit(Vector3 hitPoint, Vector3 hitNormal, bool isDamageable) =>
        StopFly(hitPoint, hitNormal);

    private void StopFly(Vector3 hitPoint, Vector3 hitNormal)
    {
        _trail.Clear();
        _trail.enabled = false;

        Explosion(hitPoint, hitNormal);

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        _projectilePool.Return(ProjectileType.Cannon, this);
    }

    private void Explosion(Vector3 hitPoint, Vector3 hitNormal)
    {
        RocketExplosion explosion = _poolService.Get<RocketExplosion>();

        explosion.Play(hitPoint, hitNormal);
    }
}
