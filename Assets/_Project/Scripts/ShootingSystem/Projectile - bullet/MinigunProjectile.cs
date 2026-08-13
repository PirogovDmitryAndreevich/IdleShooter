using System.Collections;
using UnityEngine;

public class MinigunProjectile : Projectile
{
    private float _flyTime = 5f;
    protected Coroutine _animationCoroutine;

    protected override void StartFly(Vector3 targetPosition)
    {
        _animationCoroutine = StartCoroutine(FlyCoroutine(targetPosition));
    }

    private IEnumerator FlyCoroutine(Vector3 b)
    {
        Vector3 startPosition = transform.position;

        for (float t = 0; t < 1; t += Time.deltaTime / _flyTime)
        {
            transform.position = Vector3.Lerp(startPosition, b, t);
            yield return null;
        }

        Die();
    }

    protected override void Die()
    {
        StopCoroutine(_animationCoroutine);

        base.Die();
    }
}