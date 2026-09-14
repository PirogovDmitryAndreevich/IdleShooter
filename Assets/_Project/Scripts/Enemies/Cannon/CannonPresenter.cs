using UnityEngine;

public class CannonPresenter : EnemyPresenter, IIdleState, IShootingAttackState
{
    private readonly EnemyDirectionOfView _directionOfView;
    private readonly RotateToTarget _characterRotation;
    private readonly CannonStateMachine _stateMachine;
    private readonly EnemyView _view;
    private readonly EnemyShooter _shooter;
    private readonly PlayerTriggerObserver _playerTriggerObserver;

    public CannonPresenter(EnemyView view, EnemyModel model, EnemyShooter shooter,
        PlayerTriggerObserver playerTriggerObserver) : base(view, model)
    {
        _view = view;
        _shooter = shooter;
        _playerTriggerObserver = playerTriggerObserver;

        Transform transform = _view.transform;

        _directionOfView = new EnemyDirectionOfView(transform, model);
        _characterRotation = new RotateToTarget(model, transform);
        _stateMachine = new CannonStateMachine(this, model);

        _playerTriggerObserver.TargetEnter += OnPlayerEnter;
        _playerTriggerObserver.PlayerExited += OnPlayerExited;
    }

    public Transform AttackTarget { get; private set; }
    public Transform Transform => _view.transform;
    public bool IsPlayerInCollider { get; private set; }

    public void Update() =>
        _stateMachine.Update();

    public override void ResetObject()
    {
        base.ResetObject();
    }

    public override void Dispose()
    {
        base.Dispose();

        _playerTriggerObserver.TargetEnter -= OnPlayerEnter;
        _playerTriggerObserver.PlayerExited -= OnPlayerExited;
    }

    // IdleState
    public void OnIdleStateEnter()
    {
        
    }

    // AttackState
    public void OnEnterAttackState() { }

    public void AttackingUpdate()
    {
        if (AttackTarget == null)
            return;

        if (_directionOfView.TrySeeTarget(AttackTarget))
            _shooter.TryShoot(AttackTarget);

        _characterRotation.UpdateRotation(AttackTarget);
    }

    public void OnExitAttackState() { }

    // Helpers
    private void OnPlayerEnter(Transform transform)
    {
        AttackTarget = transform;
        IsPlayerInCollider = true;
    }

    private void OnPlayerExited()
    {
        AttackTarget = null;
        IsPlayerInCollider = false;
    }
}
