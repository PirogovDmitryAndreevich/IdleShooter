using UnityEngine;

public class DemonPresenter : EnemyPresenter
{
    private readonly DemonView _view;
    private readonly PlayerTriggerObserver _playerTriggerObserver;
    private readonly DemonStateMachine _stateMachine;
    private readonly AgentMoveToTarget _agentMoveToTarget;
    private readonly Transform _spawnPoint;

    public DemonPresenter(
        EnemyModel model,
        DemonView view,
        Transform spawnPoint,
        PlayerTriggerObserver playerTriggerObserver) : base(view, model)
    {
        _view = view;
        _spawnPoint = spawnPoint;
        _playerTriggerObserver = playerTriggerObserver;

        _view.DamageCollider.Construct(model.Damage);

        _agentMoveToTarget = new AgentMoveToTarget(_view.NavMeshAgent);
        _stateMachine = new DemonStateMachine(this, model, _view.AttackEvent);

        _playerTriggerObserver.TargetEnter += OnPlayerEnter;
        _playerTriggerObserver.PlayerExited += OnPlayerExited;
    }

    public Transform Target { get; private set; }
    public bool IsPlayerInCollider { get; private set; }
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

    public void WaitState()
    {
        if (_view.NavMeshAgent.isOnNavMesh)
            _view.NavMeshAgent.ResetPath();

        _view.WaitState();
    }

    public void MoveState()
    {
        _view.Move();
    }

    public void Moving()
    {
        _agentMoveToTarget.Move(Target.position);
    }

    public void StopMove()
    {
        _view.StopMove();
        _agentMoveToTarget.StopMove();
    }

    public void AttackState()
    {
        _view.Attack();
    }

    public void OnDealDamage()
    {
       _view.OnDealDamage();
    }

    public void OnAttackEnded()
    {
        _view.OnAttackEnded();
    }

    private void OnPlayerEnter(Transform transform)
    {
        Target = transform;
        IsPlayerInCollider = true;
    }

    private void OnPlayerExited()
    {
        Target = _spawnPoint;
        IsPlayerInCollider = false;
    }
}