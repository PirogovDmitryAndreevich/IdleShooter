using System.Collections;
using UnityEngine;

public class Stone : Projectile
{
    [Header("Stone Settings")]
    [SerializeField] protected AnimationCurve FlyCurve;
    [SerializeField] protected float FlyHeight;
    [SerializeField] private float _flySpeed = 5f;

    protected Coroutine _animationCoroutine;

    protected override void StartFly(Vector3 startPosition, Vector3 targetPosition)
    {
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

        FinishFlight();
    }

    protected override void StopFly()
    {
        if (_animationCoroutine == null)
            return;

        StopCoroutine(_animationCoroutine);
        _animationCoroutine = null;
    }
}
