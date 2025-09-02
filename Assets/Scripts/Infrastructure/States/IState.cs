namespace Infrastructure.States
{
    public interface IState : IExitableState
    {
        public void Exit();

        public void Enter();
    }

    public interface IPayloadState<TPayload> : IExitableState
    {
        public void Exit();

        public void Enter(TPayload payload);
    }

    public interface IExitableState
    {
        public void Exit();
    }
}