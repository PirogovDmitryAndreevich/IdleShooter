using System;
using System.Collections.Generic;

class DemonStateMachine : CharacterStateMachine
{
    private readonly DemonPresenter _demon;

    public DemonStateMachine(DemonPresenter demon, EnemyModel model, AnimationEvent animationEvent)
    {
        _demon = demon;

        States = new Dictionary<Type, CharacterState>()
        {
            [typeof(DemonIdleState)] = new DemonIdleState(this, _demon),
            [typeof(DemonMoveState)] = new DemonMoveState(this, _demon, model),
            [typeof(DemonAttackState)] = new DemonAttackState(this, _demon, animationEvent)
        };

        ChangeState<DemonIdleState>();
    }
}
