using SimpleInjector;

namespace DefaultNamespace
{
    public class GameSessionModel
    {
        public readonly float maxSpawnDelay = 0.4f;
        public readonly float defaultSpawnDelay = 2.2f;
        private readonly int _failItemsLimit = 3;
        private int _caughtItemsAmount;
        private int _failItemsAmount;
        
        private readonly ItemsService _itemsService;
        private readonly StateMachine _stateMachine;

        public GameSessionModel(Container dependencyInjectionContainer)
        {
            _itemsService = dependencyInjectionContainer.GetInstance<ItemsService>();
            _stateMachine = dependencyInjectionContainer.GetInstance<StateMachine>();
        }

        public int caughtItemsAmount => _caughtItemsAmount;

        public int failItemsAmount => _failItemsAmount;
        
        public ItemModel GetGameItem() => _itemsService.GetGameItem();

        public void SettingsButtonClick()
        {
            _stateMachine.SetState(StateName.SettingsState);
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