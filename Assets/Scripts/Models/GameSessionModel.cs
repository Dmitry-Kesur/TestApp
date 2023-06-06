using System;
using SimpleInjector;

namespace DefaultNamespace
{
    public class GameSessionModel
    {
        public Action OnSettingsButtonClickAction;
        public readonly float maxSpawnDelay = 0.4f;
        public readonly float defaultSpawnDelay = 2.2f;
        private readonly int _failItemsLimit = 3;
        private int _catchItemsAmount;
        private int _failItemsAmount;
        private bool _isPause;
        
        private readonly ItemsService _itemsService;

        public GameSessionModel(Container dependencyInjectionContainer)
        {
            _itemsService = dependencyInjectionContainer.GetInstance<ItemsService>();
        }

        public bool isPause
        {
            get => _isPause;
            set => _isPause = value;
        }
        
        public int catchItemsAmount => _catchItemsAmount;

        public int failItemsAmount => _failItemsAmount;
        
        public ItemModel GetGameItem() => _itemsService.GetGameItem();

        public void SettingsButtonClick()
        {
            OnSettingsButtonClickAction?.Invoke();
        }

        public void IncreaseCatchItemsAmount()
        {
            _catchItemsAmount++;
        }
        
        public void IncreaseFailItemsAmount()
        {
            _failItemsAmount++;
        }

        public void Reset()
        {
            _failItemsAmount = 0;
            _catchItemsAmount = 0;
        }

        public bool IsFailItemsLimitReached() => _failItemsAmount == _failItemsLimit;
    }
}