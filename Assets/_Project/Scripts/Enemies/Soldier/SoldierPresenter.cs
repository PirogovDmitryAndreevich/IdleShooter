using UnityEngine;

public class SoldierPresenter : EnemyPresenter, IMoveState,
    IIdleState, IShootingAttackState
{
    private readonly SoldierView _view;
    private readonly PlayerTriggerObserver _triggerObserver;
    private readonly EnemyShooter _shooter;
    private readonly RotateToTarget _characterRotation;
    private readonly EnemyDirectionOfView _directionOfView;
    private readonly AgentMoveToTarget _agentMoveToTarget;

    private readonly Transform _waitPoint;
    private readonly Transform _attackPoint;

    public Transform MoveTarget { get; private set; }
    public Transform AttackTarget { get; private set; }
    public Transform Transform => _view.transform;
    public bool IsPlayerInCollider { get; private set; }


    public SoldierPresenter(
        EnemyModel model,
        SoldierView view,
        Transform waitPoint,
        Transform attackPoint,
        EnemyShooter shooter,
        PlayerTriggerObserver triggerObserver)
        : base(view, model, view.Sounds)
    {
        _view = view;
        _waitPoint = waitPoint;
        _attackPoint = attackPoint;
        _shooter = shooter;
        _triggerObserver = triggerObserver;

        Transform transform = _view.transform;

        _directionOfView = new EnemyDirectionOfView(transform, model);
        _characterRotation = new RotateToTarget(model, transform);
        _agentMoveToTarget = new AgentMoveToTarget(_view.NavMeshAgent);

        StateMachine = new SoldierStateMachine(this, model);

        _triggerObserver.TargetEnter += OnPlayerEnter;
        _triggerObserver.PlayerExited += OnPlayerExited;
    }


    public override void Dispose()
    {
        base.Dispose();

        _triggerObserver.TargetEnter -= OnPlayerEnter;
        _triggerObserver.PlayerExited -= OnPlayerExited;

    }

    public override void ResetObject()
    {
        base.ResetObject();
    }

    // IdleState
    public void OnIdleStateEnter() => 
        _view.Idle();

    // MoveState
    public void OnMoveStateEnter() => 
        _view.Move();

    public void MovingUpdate() => 
        _agentMoveToTarget.Move(MoveTarget.position);

    public void OnMoveStateExit()
    {
        _view.StopMove();
        _agentMoveToTarget.StopMove();
    }

    // AttackState
    public void OnEnterShootState() => 
        _view.Shooting();

    public void ShootingUpdate()
    {
        if(AttackTarget == null)
            return;

        if (_directionOfView.TrySeeTarget(AttackTarget))
            _shooter.TryShoot(AttackTarget);

        _characterRotation.UpdateRotation(AttackTarget);
    }

    public void OnExitShootState() => 
        _view.StopShooting();

    // Helpers
    private void OnPlayerEnter(Transform transform)
    {
        AttackTarget = transform;
        MoveTarget = _attackPoint;
        IsPlayerInCollider = true;
    }

    private void OnPlayerExited()
    {
        AttackTarget = null;
        MoveTarget = _waitPoint;
        IsPlayerInCollider = false;
    }
}
