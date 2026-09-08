using System.Collections.Generic;

class ZombieStateMachine : CharacterStateMachine
{
    private readonly ZombiePresenter _zombie;

    public ZombieStateMachine(ZombiePresenter zimbie, EnemyModel model, AnimationEvent animationEvent)
    {
        _zombie = zimbie;

        States = new Dictionary<StatesType, CharacterState>()
        {
            [StatesType.Attack] = new AttackWithAnimationEventState(this, _zombie, animationEvent),
            [StatesType.Idle] = new IdleState(this, _zombie),
            [StatesType.Move] = new MoveState(this, _zombie, model)
        };

        ChangeState(StatesType.Idle);
    }
}