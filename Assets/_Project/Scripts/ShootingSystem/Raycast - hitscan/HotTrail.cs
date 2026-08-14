using System.Collections;
using UnityEngine;

public class HotTrail : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private TrailRenderer _trail;

    private IPoolService _pool;

    public void Construct(IPoolService pool) => 
        _pool = pool;

    public void StartFly(Vector3 startPoint, RaycastHit hit)
    {
        _trail.Clear();
        _trail.enabled = false;

        StartCoroutine(FlyCoroutine(startPoint, hit));
    }

    private IEnumerator FlyCoroutine(Vector3 startPoint, RaycastHit hit)
    {
        float distance = Vector3.Distance(startPoint, hit.point);
        float duration = distance / _speed;
        float time = 0f;

        transform.position = startPoint;

        _trail.Clear();
        _trail.enabled = true;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            transform.position = Vector3.Lerp(startPoint, hit.point, t);

            yield return null;
        }

        transform.position = hit.point;

        var bulletHole = _pool.Get<BulletHole>();
        bulletHole.Show(hit.point, hit.normal);

        _pool.Return(this);
    }
}
