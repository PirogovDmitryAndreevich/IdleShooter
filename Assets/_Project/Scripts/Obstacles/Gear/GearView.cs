using System.Collections;
using UnityEngine;

public class GearView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private DamageableCollider _damageableCollider;
    [SerializeField] private DamageCollider _damageCollider;

    private Coroutine _moving;

    public HealthBar HealthBar =>
        _healthBar;
    public DamageableCollider DamageableCollider =>
        _damageableCollider;
    public DamageCollider DamageCollider =>
        _damageCollider;

    public void OnDied()
    {
        StopMoving();

        gameObject.SetActive(false);
    }

    public void OnReset()
    {
        StopMoving();

        gameObject.SetActive(true);
    }

    public void StartMoving(Vector3 endPosition, float speed)
    {
        StopMoving();

        _moving = StartCoroutine(Moving(transform.position, endPosition, speed));
    }

    private IEnumerator Moving(Vector3 startPosition, Vector3 endPosition, float speed)
    {
        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / speed;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }

        transform.position = endPosition;
        _moving = null;
    }

    private void StopMoving()
    {
        if (_moving != null)
        {
            StopCoroutine(_moving);
            _moving = null;
        }
    }
}
