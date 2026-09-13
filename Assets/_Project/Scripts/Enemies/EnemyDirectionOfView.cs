using UnityEngine;

public class EnemyDirectionOfView
{
    private readonly Transform _transform;
    private readonly EnemyModel _model;

    public EnemyDirectionOfView(Transform enemyTransform, EnemyModel model)
    {
        _transform = enemyTransform;
        _model = model;
    }

    public bool TrySeeTarget(Transform target)
    {
        const float EyeHeight = 0.7f;

        Vector3 origin = _transform.position + Vector3.up * EyeHeight;
        Vector3 targetPosition = target.position + Vector3.up * EyeHeight;

        Vector3 direction = targetPosition - origin;
        float distance = direction.magnitude;

        if (distance > _model.ShootRange)
        {
            Debug.DrawLine(origin, targetPosition, Color.white);

            return false;
        }

        if (!Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance))
        {
            Debug.DrawLine(origin, targetPosition, Color.white);
            return false;
        }

        bool seesTarget =
            hit.transform == target ||
            hit.transform.IsChildOf(target);

        Debug.DrawLine(
            origin,
            hit.point,
            seesTarget ? Color.red : Color.white);

        return seesTarget;
    }
}