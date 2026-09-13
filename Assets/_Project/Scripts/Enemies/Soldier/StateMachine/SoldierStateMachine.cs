using System.Collections.Generic;

class SoldierStateMachine : CharacterStateMachine
{
    private readonly SoldierPresenter _soldier;

    public SoldierStateMachine(SoldierPresenter soldier, EnemyModel model)
    {
        _soldier = soldier;

        States = new Dictionary<StatesType, CharacterState>()
        {
            [StatesType.Idle] = new IdleState(this, model, _soldier, _soldier),
            [StatesType.Move] = new MoveState(this, _soldier, model),
            [StatesType.Attack] = new ShootingAttackState(this, _soldier)
        };

        ChangeState(StatesType.Idle);
    }
}