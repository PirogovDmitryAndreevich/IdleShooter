using System.Collections.Generic;

class RobotStateMachine : CharacterStateMachine
{
    private readonly RobotPresenter _robot;

    public RobotStateMachine(RobotPresenter robot, EnemyModel model, AnimationEvent animationEvent)
    {
        _robot = robot;

        States = new Dictionary<StatesType, CharacterState>()
        {
            [StatesType.Kick] = new KickAttackWithAnimationEventState(this, _robot, animationEvent),
            [StatesType.Shoot] = new RobotShootState(this, _robot, _robot),
            [StatesType.Idle] = new RobotIdleState(this, model, _robot, _robot, _robot, _robot, _robot),
            [StatesType.Move] = new RobotMoveState(this, _robot, model, _robot, _robot),
            [StatesType.TakeDamage] = new RobotTakeDamageState(this, _robot)
        };

        ChangeState(StatesType.Idle);
    }
}