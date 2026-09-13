using System;
using System.Collections.Generic;

class DemonStateMachine : CharacterStateMachine
{
    private readonly DemonPresenter _demon;

    public DemonStateMachine(DemonPresenter demon, EnemyModel model, AnimationEvent animationEvent)
    {
        _demon = demon;

        States = new Dictionary<StatesType, CharacterState>()
        {
            [StatesType.Idle] = new IdleState(this, model, _demon, _demon),
            [StatesType.Move] = new MoveState(this, _demon, model),
            [StatesType.Attack] = new AttackWithAnimationEventState(this, _demon, animationEvent)
        };

        ChangeState(StatesType.Idle);
    }
}
