using System;
using UnityEngine;

public class SpikeCylinderPresenter : IDisposable, IResetObject
{
    private readonly SpikeCylinderModel _model;
    private readonly SpikeCylinderView _view;
    private readonly ImpactPunchScale _impactView;

    public bool IsNeedReset => throw new NotImplementedException();

    public SpikeCylinderPresenter(SpikeCylinderModel model, SpikeCylinderView view)
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
        _view.HitFlash.Flash();

        if (_model.Health.CurrentHealth <= 0)
            Died();
    }

    private void Died() =>
        _view.OnDied();

    public void ResetObject()
    {
        _view.OnReset();
        _model.Health.Reset();
    }
}