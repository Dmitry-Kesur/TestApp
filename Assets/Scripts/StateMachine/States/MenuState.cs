namespace DefaultNamespace.States
{
    public class MenuState : BaseState
    {
        private readonly GameHandler _gameHandler;
        
        public MenuState(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }
        
        public override void OnStateEnter()
        {
            _gameHandler.interfaceModel.ShowWindow(new MenuWindowModel(_gameHandler));
        }
    }
}