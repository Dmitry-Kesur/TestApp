using DefaultNamespace.States;
using SimpleInjector;

namespace DefaultNamespace
{
    public class StateFactory
    {
        public static BaseState CreateState(StateType stateType, Container container)
        {
            BaseState state = null;
            
            if (stateType == StateType.InitState)
            {
                state = new InitState(stateType, container);
            }
            else if (stateType == StateType.MenuState)
            {
                state = new MenuState(stateType, container);
            }
            else if (stateType == StateType.GameSession)
            {
                state = new GameSessionState(stateType, container);
            }
            else if (stateType == StateType.SettingsState)
            {
                state = new SettingsState(stateType, container);
            }

            return state;
        }
    }
}