using System.Collections;
using UnityEngine;

public class Stone : Projectile
{
    [Header("Stone Settings")]
    [SerializeField] protected AnimationCurve FlyCurve;
    [SerializeField] protected float FlyHeight;

    private float _flyTime = 5f;

    protected Coroutine _animationCoroutine;

    protected override void StartFly(Vector3 targetPosition)
    {
        _animationCoroutine = StartCoroutine(FlyCoroutine(targetPosition));
    }

    private IEnumerator FlyCoroutine(Vector3 b)
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, b);

        float flyTime = distance / _flyTime;

        for (float t = 0; t < 1; t += Time.deltaTime / flyTime)
        {
            Vector3 horizontalPosition = Vector3.Lerp(startPosition, b, t);
            float curveHeight = FlyCurve.Evaluate(t);

            transform.position = horizontalPosition + Vector3.up * curveHeight;
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
