namespace DefaultNamespace
{
    public enum StateName
    {
        GameSession,
        MenuState,
        SettingsState
    }

    public abstract class BaseState
    {
        public StateMachine stateMachine;

        public virtual void OnStateEnter()
        {
        }
    }
}