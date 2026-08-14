using System.Collections;
using UnityEngine;

public class MinigunProjectile : Projectile
{
    [SerializeField] private float _flySpeed = 5f;

    private Coroutine _animationCoroutine;

    protected override void StartFly(Vector3 startPosition ,Vector3 targetPosition)
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

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
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