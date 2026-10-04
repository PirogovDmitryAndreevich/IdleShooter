using System;
using UnityEngine;

public abstract class EnemyPresenter : IDisposable, IResetObject, IPlayerTarget
{
    protected CharacterStateMachine StateMachine;

    private readonly EnemyView _view;
    private readonly EnemyModel _model;
    private readonly Health _health;

    private bool _isDied;

    public bool IsDestroyed => _isDied;

    public EnemyPresenter(EnemyView view, EnemyModel model)
    {
        _view = view;
        _model = model;

        _isDied = false;

        _health = new Health(_model.HealthPoint);

        _view.HealthBar.Construct(_health);
        _view.DamageableCollider.DamageReceived += TakeDamage;

        _health.Died += OnDied;
    }

    public virtual void Dispose()
    {
        _view.DamageableCollider.DamageReceived -= TakeDamage;
    }

    public virtual void ResetObject()
    {
        _isDied = false;
        _health.Reset();
        _view.gameObject.SetActive(true);
    }

    public virtual void Update()
    {
        if (_isDied)
            return;

        StateMachine.Update();
    }

    protected virtual void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _view.HitEffect.Flash();

        _health.TakeDamage(damage);
    }

    protected virtual void OnDied()
    {
        _isDied = true;
        _view.gameObject.SetActive(false);
    }
}