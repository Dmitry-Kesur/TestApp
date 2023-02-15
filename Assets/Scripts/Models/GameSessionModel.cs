namespace DefaultNamespace
{
    public class GameSessionModel
    {
        private readonly GameHandler _gameHandler;

        public GameSessionModel(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }

        public ItemModel GetItem() => _gameHandler.itemsHandler.GetSelectedItem();

        public void SettingsButtonClick()
        {
            _gameHandler.stateMachine.SetState(StateName.SettingsState);
        }
    }
}