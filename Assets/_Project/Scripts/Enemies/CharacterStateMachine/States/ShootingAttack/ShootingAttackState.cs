using UnityEngine;

class ShootingAttackState : CharacterState
{
    private readonly IShootingAttackState _shootingAttack;

    public ShootingAttackState(CharacterStateMachine stateMachine, IShootingAttackState shootingAttack)
        : base(stateMachine)
    {
        _shootingAttack = shootingAttack;

        Transitions = new CharacterTransition[]
        {
            new PlayerExitedTransition(stateMachine, shootingAttack),
        };
    }

    public override StatesType Type => StatesType.Attack;

    public override void Enter() =>
        _shootingAttack.OnEnterShootState();

    public override void Update() =>
        _shootingAttack.ShootingUpdate();

    public override void Exit() =>
        _shootingAttack.OnExitShootState();
}
