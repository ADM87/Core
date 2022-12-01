namespace ADM.Core
{
    public abstract class SimpleState<TContext> : 
        IState<TContext>
    {
        private TContext m_Context;
        public TContext Context => m_Context;

        public virtual void Enter(IStatePayload payload = default) { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }

        public void SetContext(TContext context)
            => m_Context = context;
    }
}
