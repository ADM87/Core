namespace ADM.Core
{
    public interface IState<TContext>
    {
        TContext Context { get; }

        void SetContext(TContext context);

        void Enter(IStatePayload payload = default);

        void Exit();

        void Update();

        void FixedUpdate();

        void LateUpdate();
    }
}
