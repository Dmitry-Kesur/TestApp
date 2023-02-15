using DefaultNamespace;

public class GameSessionState : BaseState
{
    private readonly GameHandler _gameHandler;
    private readonly GameSessionModel _gameSessionModel;
    
    public GameSessionState(GameHandler gameHandler)
    {
        _gameHandler = gameHandler;
        _gameSessionModel = new GameSessionModel(_gameHandler);
    }

    public override void OnStateEnter()
    {
        _gameHandler.interfaceModel.ShowGameSessionInterface(_gameSessionModel);
    }
}