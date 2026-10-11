using UnityEngine;

public class RobotPresenter : EnemyPresenter, IIdleState, IMoveState,
    IAttackWithAnimationEvent, IShootingAttackState, IWaitWeaponReload,
    ITakingDamage
{
    private readonly RobotView _view;
    private readonly EnemyModel _model;
    private readonly Transform _waitPoint;
    private readonly EnemyShooter _shooter;
    private readonly PlayerTriggerObserver _playerTriggerObserver;
    private readonly RotateToTarget _characterRotation;
    private readonly EnemyDirectionOfView _directionOfView;
    private readonly AgentMoveToTarget _agentMoveToTarget;

    private float _shootTimer;
    private float _reloadTimer;

    public RobotPresenter(RobotView view, EnemyModel model,
        Transform waitPoint,
        PlayerTriggerObserver playerTriggerObserver,
        EnemyShooter shooter) : base(view, model, view.Sounds)
    {
        _view = view;
        _model = model;
        _waitPoint = waitPoint;
        _playerTriggerObserver = playerTriggerObserver;
        _shooter = shooter;

        _view.DamageCollider.Construct(model.Damage);

        Transform transform = _view.transform;

        _directionOfView = new EnemyDirectionOfView(transform, model);
        _characterRotation = new RotateToTarget(model, transform);
        _agentMoveToTarget = new AgentMoveToTarget(_view.NavMeshAgent);

        StateMachine = new RobotStateMachine(this, model, view.AnimationEvent);

        _playerTriggerObserver.TargetEnter += OnPlayerEnter;
        _playerTriggerObserver.PlayerExited += OnPlayerExited;

        _reloadTimer = _model.ReloadTime;

        _view.StepEvent.Step += OnStep;
    }

    public bool IsPlayerInCollider { get; private set; }

    public Transform MoveTarget { get; private set; }

    public Transform AttackTarget { get; private set; }

    public bool IsCanShoot => IsReloading();

    public bool IsTakingDamageCompleted { get; private set; }

    public bool IsTakedDamage { get; private set; }

    public Transform Transform => _view.transform;

    public override void ResetObject()
    {
        base.ResetObject();
    }

    public override void Dispose()
    {
        base.Dispose();

        _playerTriggerObserver.TargetEnter -= OnPlayerEnter;
        _playerTriggerObserver.PlayerExited -= OnPlayerExited;

        _view.StepEvent.Step -= OnStep;
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
        _view.Kick();

    public void OnDealDamage()
    {
        _view.SoundsHandler.PlayRandomPithSound(_view.Sounds.Kick);
        _view.OnDealDamage();
    }

    public void OnAttackEnded() =>
        _view.OnAttackEnded();

    // ShootState
    public void OnEnterShootState()
    {
        _shootTimer = _model.ShootTime;

        _view.Shooting();
    }

    public void ShootingUpdate()
    {
        if (AttackTarget == null || !IsCanShoot)
            return;

        if (_directionOfView.TrySeeTarget(AttackTarget))
            _shooter.TryShoot(AttackTarget);

        _shootTimer -= Time.deltaTime;

        if (_shootTimer <= 0f)
            _reloadTimer = _model.ReloadTime;

        _characterRotation.UpdateRotation(AttackTarget);
    }

    public void OnExitShootState()
    {
        _view.StopShooting();

        _shootTimer = 0f;
    }

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
        AttackTarget = null;
        IsPlayerInCollider = false;
    }

    private bool IsReloading()
    { 
        if (_reloadTimer >= 0f)
        {
            _reloadTimer -= Time.deltaTime;

            return false;
        }

        return true;
    }

    private void OnStep() =>
        _view.SoundsHandler
        .PlayRandomPithSound(_view.Sounds.Step);
}
