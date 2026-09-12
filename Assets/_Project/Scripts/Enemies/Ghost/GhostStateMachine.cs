using System.Collections.Generic;

class GhostStateMachine : CharacterStateMachine
{
    private readonly GhostPresenter _ghost;

    public GhostStateMachine(GhostPresenter ghost, EnemyModel model, AnimationEvent animationEvent)
    {
        _ghost = ghost;

        States = new Dictionary<StatesType, CharacterState>()
        {
            [StatesType.Attack] = new AttackWithAnimationEventState(this, _ghost, animationEvent),
            [StatesType.Idle] = new IdleState(this, _ghost),
            [StatesType.Move] = new MoveState(this, _ghost, model)
        };

        ChangeState(StatesType.Idle);
    }
}