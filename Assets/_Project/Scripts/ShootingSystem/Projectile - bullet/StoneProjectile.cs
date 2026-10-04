using System.Collections;
using UnityEngine;

public class StoneProjectile : Projectile
{
    [Header("Stone Settings")]
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] protected AnimationCurve FlyCurve;
    [SerializeField] protected float FlyHeight;
    [SerializeField] private float _flySpeed = 5f;

    protected Coroutine _animationCoroutine;
    private IProjectilePool _projectilePool;

    public void Construct(IProjectilePool projectilePool)
    {
        _projectilePool = projectilePool;
    }

    protected override void OnShoot(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;

        _trail.Clear();
        _trail.enabled = true;

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

            Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, t);

            float verticalPosition =
                FlyCurve.Evaluate(t) * FlyHeight;

            transform.position = horizontalPosition + Vector3.up * verticalPosition;
            yield return null;
        }

        StopFly();
    }

    protected override void Hit(Vector3 hitPoint, Vector3 hitNormal, bool isDamageable)
    {
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

        _projectilePool.Return(ProjectileType.Stone, this);
    }
}
