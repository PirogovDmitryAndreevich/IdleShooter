using System;
using UnityEngine;

public class GearPresenter : IDisposable, IResetObject
{
    private readonly GearModel _model;
    private readonly GearView _view;

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
    }

    private void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _model.Health.TakeDamage(damage);

        if (_model.Health.CurrentHealth <= 0)
            Died();
    }

    private void Died() =>
        _view.OnDied();

    public void ResetObject() =>
        _view.OnReset();
}
