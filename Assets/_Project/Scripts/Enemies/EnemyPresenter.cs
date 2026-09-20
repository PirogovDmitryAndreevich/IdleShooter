using System;
using UnityEngine;

public class EnemyPresenter : IDisposable, IResetObject
{
    private readonly EnemyView _view;
    private readonly EnemyModel _model;
    private readonly Health health;

    public EnemyPresenter(EnemyView view, EnemyModel model)
    {
        _view = view;
        _model = model;

        health = new Health(_model.HealthPoint);

        _view.HealthBar.Construct(health);
        _view.DamageableCollider.DamageReceived += TakeDamage;
    }

    public bool IsNeedReset { get; private set; }

    public virtual void Dispose()
    {
        _view.DamageableCollider.DamageReceived -= TakeDamage;
    }

    public virtual void ResetObject()
    {
        
    }

    protected virtual void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _view.HitEffect.Flash();
    }
}