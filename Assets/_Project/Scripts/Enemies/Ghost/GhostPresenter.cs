using UnityEngine;

public class GhostPresenter : EnemyPresenter, IAttackWithAnimationEvent,
    IIdleState, IMoveState
{
    private readonly GhostView _view;
    private readonly EnemyModel _model;
    private readonly Transform _spawnPoint;
    private readonly PlayerTriggerObserver _playerTriggerObserver;
    private readonly AgentMoveToTarget _agentMoveToTarget;

    private float _stepInterval;
    private float _nextStepTime;

    public GhostPresenter(GhostView view, EnemyModel model,Transform spawnPoint,
        PlayerTriggerObserver playerTriggerObserver, ICrownsManager crownsManager) 
        : base(view, model, view.Sounds, crownsManager)
    {
        _view = view;
        _model = model;
        _spawnPoint = spawnPoint;
        _playerTriggerObserver = playerTriggerObserver;

        _view.DamageCollider.Construct(model.Damage);

        _agentMoveToTarget = new AgentMoveToTarget(_view.NavMeshAgent);
        StateMachine = new GhostStateMachine(this, _model, _view.AttackEvent);

        _stepInterval = _view.Sounds.Step.length;

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
    public void OnMoveStateEnter()
    {
        _nextStepTime = 0f;
        _view.Move();
    }

    public void MovingUpdate()
    {
        if (_nextStepTime < Time.time)
        {
            _view.SoundsHandler.PlaySound(_view.Sounds.Step);
            _nextStepTime = Time.time + _stepInterval;
        }

        _agentMoveToTarget.Move(MoveTarget.position);
    }

    public void OnMoveStateExit()
    {
        _nextStepTime = 0f;
        _view.StopMove();
        _agentMoveToTarget.StopMove();
    }


    // AttackState
    public void OnAttackStateEnter()
    {
        _view.SoundsHandler
            .PlayRandomPithSound(_view.Sounds.AttackSound);

        _view.Attack();
    }

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
