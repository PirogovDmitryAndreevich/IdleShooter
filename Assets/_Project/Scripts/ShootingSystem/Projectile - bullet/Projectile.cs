using System.Collections;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve _flyCurve;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private float _flyHeight;

    private IPoolService _pool;
    private Coroutine _animationCoroutine;
    private float _flyTime = 5f;
    private float _damage;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        BaseDamageable damageable = collision.collider.GetComponent<BaseDamageable>();

        if (damageable == null)
        {
            damageable = collision.collider.GetComponentInParent<BaseDamageable>();

            if (damageable == null)
                damageable = collision.collider.GetComponentInChildren<BaseDamageable>();
        }

        if (damageable != null)        
            damageable.TakeDamage(_damage, hitPoint, hitNormal);

        Die();
    }

    public void Construct(IPoolService poolService)
    {
        _pool = poolService;
    }

    public void Shoot(float damage, Vector3 targetPosition)
    {
        _damage = damage;

        _trail.Clear();
        _trail.enabled = true;

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
            float curveHeight = _flyCurve.Evaluate(t);

            transform.position = horizontalPosition + Vector3.up * curveHeight;
            yield return null;
        }

        Die();
    }

    private void Die()
    {
        StopCoroutine(_animationCoroutine);

        _trail.Clear();
        _trail.enabled = false;

        _pool.Return(this);
    }
}
