using System;
using UnityEngine;

public class EnemyPresenter : IDisposable, IResetObject
{
    private readonly EnemyView _view;

    public EnemyPresenter(EnemyView view)
    {
        _view = view;

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

    private void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _view.HitEffect.Flash();
    }
}