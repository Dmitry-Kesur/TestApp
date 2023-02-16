namespace DefaultNamespace
{
    public class GameSessionModel
    {
        public readonly float maxSpawnDelay = 0.4f;
        public readonly float defaultSpawnDelay = 2.2f;
        private int _failItemsLimit = 3;
        private int _caughtItemsAmount;
        private int _failItemsAmount;
        
        private readonly GameHandler _gameHandler;

        public GameSessionModel(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }

        public int caughtItemsAmount => _caughtItemsAmount;

        public int failItemsAmount => _failItemsAmount;
        
        public ItemModel GetItem() => _gameHandler.itemsHandler.GetSelectedItem();

        public void SettingsButtonClick()
        {
            _gameHandler.stateMachine.SetState(StateName.SettingsState);
        }

        public void IncreaseCaughtItemsAmount()
        {
            _caughtItemsAmount++;
        }
        
        public void IncreaseFailItemsAmount()
        {
            _failItemsAmount++;
        }

        public void OnStopGameSession()
        {
            _failItemsAmount = 0;
            _caughtItemsAmount = 0;
        }

        public bool IsFailItemsLimitReached() => _failItemsAmount == _failItemsLimit;
    }
}