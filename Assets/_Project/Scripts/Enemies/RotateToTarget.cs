using UnityEngine;

public class RotateToTarget
{
    private readonly EnemyModel _model;
    private readonly Transform _characterTransform;

    public RotateToTarget(EnemyModel model, Transform transform)
    {
        _model = model;
        _characterTransform = transform;
    }

    public void UpdateRotation(Transform target)
    {
        if (target == null)
            return;
        
        Vector3 direction = target.position - _characterTransform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        _characterTransform.rotation = 
            Quaternion.Slerp( 
                _characterTransform.rotation,
                targetRotation,
                _model.RotateSpeed * Time.deltaTime);
    }
}