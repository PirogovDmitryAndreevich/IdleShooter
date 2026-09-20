using UnityEngine;

public class RobotPresenter : EnemyPresenter, IIdleState, IMoveState,
    IAttackWithAnimationEvent, IShootingAttackState, IWaitWeaponReload,
    ITakingDamage
{
    private readonly RobotView _view;
    private readonly Transform _waitPoint;
    private readonly RobotStateMachine _stateMachine;
    private readonly EnemyShooter _shooter;
    private readonly PlayerTriggerObserver _playerTriggerObserver;
    private readonly RotateToTarget _characterRotation;
    private readonly EnemyDirectionOfView _directionOfView;
    private readonly AgentMoveToTarget _agentMoveToTarget;

    public RobotPresenter(RobotView view, EnemyModel model,
        Transform waitPoint,
        PlayerTriggerObserver playerTriggerObserver,
        EnemyShooter shooter) : base(view, model)
    {
        _view = view;
        _waitPoint = waitPoint;
        _playerTriggerObserver = playerTriggerObserver;
        _shooter = shooter;

        Transform transform = _view.transform;

        _directionOfView = new EnemyDirectionOfView(transform, model);
        _characterRotation = new RotateToTarget(model, transform);
        _agentMoveToTarget = new AgentMoveToTarget(_view.NavMeshAgent);

        _stateMachine = new RobotStateMachine(this, model, view.AnimationEvent);

        _playerTriggerObserver.TargetEnter += OnPlayerEnter;
        _playerTriggerObserver.PlayerExited += OnPlayerExited;
    }

    public bool IsPlayerInCollider { get; private set; }

    public Transform MoveTarget { get; private set; }

    public Transform AttackTarget { get; private set; }

    public bool IsCanShoot { get; private set; } /// Сделать Reload

    public bool IsTakingDamageCompleted { get; private set; }

    public bool IsTakedDamage { get; private set; }

    public Transform Transform => _view.transform;

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
        if (_view.NavMeshAgent.isOnNavMesh)
            _view.NavMeshAgent.ResetPath();

        _view.Idle();
    }

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

    // KickAttack
    public void OnAttackStateEnter() =>
        _view.Attack();

    public void OnDealDamage() =>
        _view.OnDealDamage();

    public void OnAttackEnded() =>
        _view.OnAttackEnded();

    // ShootState
    public void OnEnterShootState() =>
        _view.Shooting();

    public void ShootingUpdate()
    {
        if (AttackTarget == null)
            return;

        if (_directionOfView.TrySeeTarget(AttackTarget))
            _shooter.TryShoot(AttackTarget);

        _characterRotation.UpdateRotation(AttackTarget);
    }

    public void OnExitShootState() =>
        _view.StopShooting();

    // TakeDamageState
    public void OnTakeDamageStateEnter()
    {
        
    }

    public void OnTakeDamageStateUpdate()
    {
        
    }

    public void OnTakeDamageStateExit()
    {
        
    }

    protected override void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        IsTakedDamage = true;
        IsTakingDamageCompleted = false;
        base.TakeDamage(damage, hitPoint, hitNormal);

        IsTakedDamage = false;
        IsTakingDamageCompleted = true;
    }

    // Helpers
    private void OnPlayerEnter(Transform transform)
    {
        MoveTarget = transform;
        AttackTarget = transform;
        IsPlayerInCollider = true;
    }

    private void OnPlayerExited()
    {
        MoveTarget = _waitPoint;
        IsPlayerInCollider = false;
    }
}
