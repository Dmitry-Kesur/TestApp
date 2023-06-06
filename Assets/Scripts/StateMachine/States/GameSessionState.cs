using DefaultNamespace;
using SimpleInjector;

public class GameSessionState : BaseState
{
    private readonly InterfaceService _interfaceService;

    public GameSessionState(StateType stateType, Container container) : base(stateType, container)
    {
        var gameSessionModel = new GameSessionModel(container)
        {
            OnSettingsButtonClickAction = OnSettingsButtonClickHandler
        };

        _interfaceService = container.GetInstance<InterfaceService>();
        _interfaceService.SetGameSessionModel(gameSessionModel);
    }

    private void OnSettingsButtonClickHandler()
    {
        ChangeStateAction?.Invoke(StateType.SettingsState);
    }

    public override void OnStateChanged(BaseState previousState)
    {
        _interfaceService.ShowGameSessionInterface();
    }
}