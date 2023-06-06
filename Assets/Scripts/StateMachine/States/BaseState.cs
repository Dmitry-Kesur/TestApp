using System;
using SimpleInjector;

namespace DefaultNamespace
{
    public enum StateType
    {
        GameSession,
        InitState,
        MenuState,
        SettingsState
    }

    public class BaseState
    {
        public Action<StateType> ChangeStateAction;
        public StateType stateType;
        protected Container container;

        protected BaseState(StateType stateType, Container container)
        {
            this.stateType = stateType;
            this.container = container;
        }

        public virtual void OnStateChanged(BaseState previousState)
        {
        }
    }
}