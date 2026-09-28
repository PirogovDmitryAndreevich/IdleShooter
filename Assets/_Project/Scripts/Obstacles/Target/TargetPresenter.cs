using System;
using UnityEngine;

public class TargetPresenter : IDisposable, IResetObject
{
    private readonly TargetModel _model;
    private readonly TargetView _view;
    private readonly ImpactPunchScale _impactView;
    public bool IsNeedReset => throw new NotImplementedException();

    public TargetPresenter(TargetModel model, TargetView view)
    {
        _model = model;
        _view = view;

        _view.HealthBar.Construct(_model.Health);

        _view.DamageableCollider.DamageReceived += TakeDamage;

        _impactView = new ImpactPunchScale(_view.transform);
    }

    public void Dispose()
    {
        _view.DamageableCollider.DamageReceived -= TakeDamage;
        _impactView.Reset();
    }

    private void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _model.Health.TakeDamage(damage);

        _impactView.Play();
        _view.HitFlash.Flash();

        if (_model.Health.CurrentHealth <= 0)
            Died();
    }

    private void Died()
    {
        _impactView.Reset();
        _view.OnDied();
    }

    public void ResetObject()
    {
        _impactView.Reset();
        _view.OnReset();
        _model.Health.Reset();
    }
}