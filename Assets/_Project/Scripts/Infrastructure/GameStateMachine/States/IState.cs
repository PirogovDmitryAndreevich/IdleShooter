public interface IState : IExitingState
{
    void Enter();
}

public interface IPayloadState<TPayload> : IExitingState
{
    void Enter(TPayload payload);
}

public interface IExitingState
{
    void Exit();
}