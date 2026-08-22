using System;
using UnityEngine;

public class GearPresenter : IDisposable, IResetObject
{
    private readonly GearModel _model;
    private readonly GearView _view;

    private FlyPlatform _flyPlatform;
    private Vector3 _endPosition;
    private bool _isDied;

    public bool IsNeedReset => throw new NotImplementedException();

    public GearPresenter(GearModel model, GearView view)
    {
        _model = model;
        _view = view;

        _view.HealthBar.Construct(_model.Health);

        _view.DamageableCollider.DamageReceived += TakeDamage;
        _view.DamageCollider.Construct(_model.Damage);
    }

    public void Dispose()
    {
        _view.DamageableCollider.DamageReceived -= TakeDamage;

        if (_flyPlatform != null)
            _flyPlatform.StartMoving -= StartMoving;
    }

    public void SetTriggerToMoving(FlyPlatform flyPlatform)
    {
        _flyPlatform = flyPlatform;

        _flyPlatform.StartMoving += StartMoving;
    }

    private void StartMoving(Vector3 endPosition)
    {
        if (_isDied)
            return;

        Vector3 targetPosition = new Vector3(endPosition.x, _view.transform.position.y, endPosition.z);

        _view.StartMoving(targetPosition, _model.MoveSpeed);
    }

    private void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (_isDied)
            return;

        _model.Health.TakeDamage(damage);

        if (_model.Health.CurrentHealth <= 0)
            Died();
    }

    private void Died()
    {
        _isDied = true;
        _view.OnDied();
    }

    public void ResetObject() =>
        _view.OnReset();
}
