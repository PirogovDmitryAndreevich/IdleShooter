class TargetReachedTransition : CharacterTransition
{
    private readonly DemonPresenter _demon;
    private readonly EnemyModel _model;

    public TargetReachedTransition(CharacterStateMachine stateMachine, DemonPresenter demon, EnemyModel model)
        : base(stateMachine)
    {
        _demon = demon;
        _model = model;
    }

    public override bool IsNeedTransit()
    {
        float sqrDistance = (_demon.Transform.position - _demon.Target.position).sqrMagnitude;

        return _demon.IsPlayerInCollider && sqrDistance <= _model.AttackDistance * _model.AttackDistance;
    }

    public override void Transit()
    {
        base.Transit();
        StateMachine.ChangeState<DemonAttackState>();
    }
}
