using UnityEngine;

public class ZombiePresenter : EnemyPresenter, IAttackWithAnimationEvent,
    IIdleState, IMoveState
{
    private readonly ZombieView _view;
    private readonly PlayerTriggerObserver _playerTriggerObserver;
    private readonly AgentMoveToTarget _agentMoveToTarget;
    private readonly Transform _spawnPoint;

    public ZombiePresenter(
        ZombieView view,
        EnemyModel model,
        Transform spawnPoint,
        PlayerTriggerObserver playerTriggerObserver) : base(view, model, view.Sounds)
    {
        _view = view;
        _spawnPoint = spawnPoint;
        _playerTriggerObserver = playerTriggerObserver;

        _view.DamageCollider.Construct(model.Damage);

        _agentMoveToTarget = new AgentMoveToTarget(_view.NavMeshAgent);
        StateMachine = new ZombieStateMachine(this, model, _view.AttackEvent);

        _playerTriggerObserver.TargetEnter += OnPlayerEnter;
        _playerTriggerObserver.PlayerExited += OnPlayerExited;
    }

    public Transform MoveTarget { get; private set; }
    public bool IsPlayerInCollider { get; private set; }

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


    // AttackState
    public void OnAttackStateEnter() =>
        _view.Attack();

    public void OnDealDamage() => 
        _view.OnDealDamage();

    public void OnAttackEnded() =>
        _view.OnAttackEnded();

    // Helpers
    private void OnPlayerEnter(Transform transform)
    {
        MoveTarget = transform;
        IsPlayerInCollider = true;
    }

    private void OnPlayerExited()
    {
        MoveTarget = _spawnPoint;
        IsPlayerInCollider = false;
    }
}