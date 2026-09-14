using System.Collections.Generic;

class CannonStateMachine : CharacterStateMachine
{
    private readonly CannonPresenter _cannon;

    public CannonStateMachine(CannonPresenter cannon, EnemyModel model)
    {

        _cannon = cannon;

        States = new Dictionary<StatesType, CharacterState>()
        {
            [StatesType.Idle] = new IdleState(this, model, _cannon),
            [StatesType.Attack] = new ShootingAttackState(this, _cannon)
        };

        ChangeState(StatesType.Idle);

    }
}
