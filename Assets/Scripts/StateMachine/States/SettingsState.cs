namespace DefaultNamespace.States
{
    public class SettingsState : BaseState
    {
        private readonly GameHandler _gameHandler;
        private readonly SettingsWindowModel _settingsWindowModel;

        public SettingsState(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
            _settingsWindowModel = new SettingsWindowModel(_gameHandler);
        }
        
        public override void OnStateEnter()
        {
            _gameHandler.interfaceModel.ShowWindow(_settingsWindowModel);
        }
    }
}