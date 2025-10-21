using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wigi.Utilities;


namespace Wigi.StateMachine
{
    public abstract class StateMachineBase<T> where T : System.Enum
    {
        protected T currentState;
        public T CurrentState => currentState;

        public virtual T NextState()
        {
            currentState = currentState.Next();
            return currentState;
        }

        public virtual void Change(T state)
        {
            currentState = state;
        }

        public abstract void Update(float dt);
    }
}
