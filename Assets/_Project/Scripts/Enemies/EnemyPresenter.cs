using System;
using UnityEngine;

public abstract class EnemyPresenter : IDisposable, IResetObject, IPlayerTarget
{
    protected readonly SoundsHandler SoundsHandler;

    protected CharacterStateMachine StateMachine;

    private readonly EnemyView _view;
    private readonly EnemySounds _sounds;
    private readonly EnemyModel _model;
    private readonly Health _health;

    private bool _isDied;

    public bool IsDestroyed => _isDied;

    public EnemyPresenter(EnemyView view, EnemyModel model, EnemySounds sounds)
    {
        _view = view;
        _model = model;
        _sounds = sounds;

        SoundsHandler = _view.SoundsHandler;

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
        _view.SoundsHandler.PlaySound(_sounds.TakeDamageSound);
        _view.HitEffect.Flash();

        _health.TakeDamage(damage);
    }

    protected virtual void OnDied()
    {
        _view.SoundsHandler.PlaySound(_sounds.DiedSound);
        _isDied = true;
        _view.gameObject.SetActive(false);
    }
}