using System.Collections.Generic;

namespace ADM.Core
{
    public class StateMachine<TContext> : Dictionary<string, IState<TContext>>
    {
        private readonly TContext m_Context;

        public string Current { get; private set; }

        public StateMachine(TContext context)
            => m_Context = context;

        public void Register(string name, IState<TContext> state)
        {
            Assert.That(!ContainsKey(name), $"Detected duplicated state keys [{name}]");
            state.SetContext(m_Context);

            Add(name, state);
        }

        public void Enter(string name, IStatePayload payload = default)
        {
            if (!TryGetValue(name, out var state))
                return;

            if (!string.IsNullOrEmpty(Current))
                this[Current].Exit();

            Current = name;
            state.Enter(payload);
        }

        public void Restart(IStatePayload payload = default)
        {
            if (string.IsNullOrEmpty(Current))
                return;

            Enter(Current, payload);
        }

        public void Exit()
        {
            if (string.IsNullOrEmpty(Current))
                return;

            this[Current].Exit();
            Current = null;
        }

        public void Update()
        {
            if (string.IsNullOrEmpty(Current))
                return;

            this[Current].Update();
        }

        public void FixedUpdate()
        {
            if (string.IsNullOrEmpty(Current))
                return;

            this[Current].FixedUpdate();
        }

        public void LateUpdate()
        {
            if (string.IsNullOrEmpty(Current))
                return;

            this[Current].LateUpdate();
        }
    }
}
