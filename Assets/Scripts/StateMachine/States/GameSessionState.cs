using DefaultNamespace;
using SimpleInjector;

public class GameSessionState : BaseState
{
    private readonly GameSessionModel _gameSessionModel;
    private readonly InterfaceService _interfaceService;

    public GameSessionState(Container dependencyInjectionContainer)
    {
        _gameSessionModel = new GameSessionModel(dependencyInjectionContainer);
        _interfaceService = dependencyInjectionContainer.GetInstance<InterfaceService>();
    }

    public override void OnStateEnter()
    {
        _interfaceService.ShowGameSessionInterface(_gameSessionModel);
    }
}