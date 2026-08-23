using System;
using UnityEngine;

public class GearBasePresenter : IDisposable, IResetObject
{
    private readonly GearBaseModel _model;
    private readonly GearBaseView _view;
    private readonly ImpactPunchScale _impactView;

    public bool IsNeedReset => throw new NotImplementedException();

    public GearBasePresenter(GearBaseModel model, GearBaseView view)
    {
        _model = model;
        _view = view;

        _view.HealthBar.Construct(_model.Health);

        _view.DamageableCollider.DamageReceived += TakeDamage;
        _view.DamageCollider.Construct(_model.Damage);

        _impactView = new ImpactPunchScale(_view.transform);
    }

    public void Dispose()
    {
        _view.DamageableCollider.DamageReceived -= TakeDamage;
    }

    private void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _model.Health.TakeDamage(damage);

        _impactView.Play();

        if (_model.Health.CurrentHealth <= 0)
            Died();
    }

    private void Died() =>
        _view.OnDied();

    public void ResetObject() =>
        _view.OnReset();
}