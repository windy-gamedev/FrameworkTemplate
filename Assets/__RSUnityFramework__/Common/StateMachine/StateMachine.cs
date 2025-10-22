using System.Collections;
using System.Collections.Generic;
using Wigi.Utilities;


namespace Wigi.StateMachine
{
    public class StateMachine<T> : StateMachineBase<T> where T : System.Enum
    {
        private IState<T> defaultContext;
        private Dictionary<T, IState<T>> contexts;

        IState<T> currentContext;

        public StateMachine(IState<T> defaultContext)
        {
            SetDefaultContext(defaultContext);
        }

        public StateMachine(Dictionary<T, IState<T>> contexts)
        {
            this.contexts = contexts;
        }

        public void SetDefaultContext(IState<T> defaultContext)
        {
            this.defaultContext = defaultContext;
        }

        public void SetContexts(Dictionary<T, IState<T>> contexts)
        {
            this.contexts = contexts;
        }

        public void AddContext(T state, IState<T> context)
        {
            contexts.Add(state, context);
        }

        public override T NextState()
        {
            Change(currentState.Next());
            return currentState;
        }

        public override void Change(T state)
        {
            //Exit old state
            if (currentContext != null)
            {
                currentContext.OnExitState(currentState);
            }
            if(defaultContext != null)
            {
                defaultContext.OnExitState(currentState);
            }

            //Enter new State
            currentState = state;
            if (contexts != null && contexts.ContainsKey(currentState))
            {
                currentContext = contexts[currentState];
            }
            else
            {
                currentContext = null;
            }

            if (currentContext != null)
            {
                currentContext.OnEnterState(currentState);
            }
            if (defaultContext != null)
            {
                defaultContext.OnEnterState(currentState);
            }
        }

        public override void Update(float dt)
        {
            if(currentContext != null)
            {
                currentContext.UpdateState(dt);
            }
        }
    }

    public interface IState<T> where T : System.Enum
    {
        void OnEnterState(T state);
        void UpdateState(float dt);
        void OnExitState(T state);
    }
}
