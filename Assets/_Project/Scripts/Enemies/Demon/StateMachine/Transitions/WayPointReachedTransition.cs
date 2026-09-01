class WayPointReachedTransition : CharacterTransition
{
    private readonly DemonPresenter _demon;
    private readonly EnemyModel _model;

    public WayPointReachedTransition(CharacterStateMachine stateMachine, DemonPresenter demon, EnemyModel model )
        : base(stateMachine)
    {
        _demon = demon;
        _model = model;
    }

    public override bool IsNeedTransit()
    {
        float sqrDistance = (_demon.Transform.position - _demon.Target.position).sqrMagnitude;       

        return !_demon.IsPlayerInCollider && sqrDistance <= _model.StopMoveDistance * _model.StopMoveDistance;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<DemonIdleState>();
    }
}
