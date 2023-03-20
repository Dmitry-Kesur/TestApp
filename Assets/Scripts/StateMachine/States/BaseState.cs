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
        public virtual void OnStateEnter()
        {
        }
    }
}