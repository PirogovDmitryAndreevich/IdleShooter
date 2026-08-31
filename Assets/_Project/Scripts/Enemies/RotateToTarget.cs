using UnityEngine;

public class RotateToTarget
{
    private readonly EnemyModel _model;
    private readonly SoldierView _view;

    private Transform _characterTransform;

    public RotateToTarget(EnemyModel model, SoldierView view)
    {
        _model = model;
        _view = view;

        _characterTransform = _view.transform;
    }

    public void UpdateRotation(Transform target)
    {
        if (target == null)
            return;
        
        Vector3 direction = target.position - _characterTransform.position;
        
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