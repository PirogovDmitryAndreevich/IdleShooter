public class SoldierIdleState : ISoldierState
{
    private readonly SoldierView _view;

    public SoldierIdleState(SoldierView view)
    {
        _view = view;
    }

    public void Enter()
    {
        if (_view.NavMeshAgent.isOnNavMesh)
            _view.NavMeshAgent.ResetPath();

        _view.SetMoveAnimation(false);
        _view.SetAttackAnimation(false);
    }

    public void Exit()
    {
    }
}
