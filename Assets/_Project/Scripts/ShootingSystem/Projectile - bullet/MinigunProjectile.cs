using System.Collections;
using UnityEngine;

public class MinigunProjectile : Projectile
{
    [Header("Minigun Projectile Settings")]
    [SerializeField] private float _flySpeed = 5f;
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

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        StopFly();
    }

    protected override void Hit(Vector3 hitPoint, Vector3 hitNormal)
    {
        BulletHole bulletHole = _poolService.Get<BulletHole>();
        BulletHitEffect effect = _poolService.Get<BulletHitEffect>();

        bulletHole.Show(hitPoint, hitNormal);
        effect.Play(hitPoint, hitNormal);

        StopFly();
    }

    private void StopFly()
    {
        _trail.Clear();
        _trail.enabled = false;

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        _projectilePool.Return(ProjectileType.Minigun, this);
    }
}
