namespace DefaultNamespace.States
{
    public class MenuState : BaseState
    {
        private readonly GameHandler _gameHandler;
        private readonly MenuWindowModel _menuWindowModel;
        
        public MenuState(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
            _menuWindowModel = new MenuWindowModel(_gameHandler);
        }
        
        public override void OnStateEnter()
        {
            _gameHandler.interfaceModel.ShowWindow(_menuWindowModel);
        }
    }
}