using System;
using UnityEngine;

public class ObstaclePresenter : IDisposable, IResetObject, IPlayerTarget
{
    private readonly ObstacleView _view;
    private readonly ObstacleModel _model;
    private readonly ImpactPunchScale _impactView;
    private readonly ObstacleType _type;
    protected bool IsDied;

    public ObstaclePresenter(ObstacleView view, ObstacleModel model, ObstacleType type)
    {
        _view = view;
        _model = model;
        _type = type;

        _impactView = new ImpactPunchScale(_view.transform);
        _view.HealthBar.Construct(_model.Health);

        if (_view.DamageCollider != null)
            view.DamageCollider.Construct(model.Damage);

        _view.DamageableCollider.DamageReceived += TakeDamage;
        _model.Health.Died += Died;
    }

    public ObstacleType Type => 
        _type;

    public ObstacleView View =>
        _view;

    public bool IsDestroyed => IsDied;

    public virtual void Dispose()
    {
        _view.DamageableCollider.DamageReceived -= TakeDamage;
        _model.Health.Died -= Died;

        _impactView.Reset();
    }

    public virtual void ResetObject()
    {
        IsDied = false;
        _model.Health.Reset();
        _view.gameObject.SetActive(true);
    }

    protected virtual void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDied)
            return;

        _model.Health.TakeDamage(damage);

        _impactView.Play();
        _view.HitFlash.Flash();

    }

    protected virtual void Died()
    {
        _impactView.Reset();
        _view.OnDied();
        IsDied = true;
    }
}
