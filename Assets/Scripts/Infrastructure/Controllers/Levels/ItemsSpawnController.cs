using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services;
using Infrastructure.Services.Items;
using Infrastructure.Services.Log;
using Infrastructure.Strategy;
using Infrastructure.Views.GameEntities;

namespace Infrastructure.Controllers.Levels
{
    public class ItemsSpawnController
    {
        private readonly List<ItemView> _items = new();

        private readonly IItemsSpawnService _itemsSpawnService;
        private readonly IItemsService _itemsService;
        private readonly IExceptionLoggerService _exceptionLoggerService;


        public Action<ItemModel> OnCatchItemAction;
        public Action OnFailItemAction;
        public Action<ItemView> OnSpawnItemAction;

        private int _catchItemsToDecreaseSpawnDelay;

        private float _spawnDelayDecreaseValue;
        private float _dropDurationDecreaseValue;
        private float _itemsSpawnDelay;
        private float _currentDropItemsDuration;
        
        private ILevelModel _levelModel;

        public ItemsSpawnController(IItemsSpawnService itemsSpawnService,
            IItemsService itemsService, IExceptionLoggerService exceptionLoggerService)
        {
            _itemsSpawnService = itemsSpawnService;
            _itemsSpawnService.OnSpawnItemAction = OnSpawnItem;
            _itemsService = itemsService;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public void SetModel(ILevelModel levelModel)
        {
            if (levelModel == null)
            {
                var exceptionText = "Level model is null";
                _exceptionLoggerService.LogError(exceptionText);
                throw new ArgumentNullException(exceptionText);
            }
            
            _levelModel = levelModel;
            AfterSetModel();
        }

        public void OnStartLevel() =>
            SpawnItems();

        public void SetItemsByIds(List<int> itemsIds)
        {
            var levelItemsStrategy = DefineItemsStrategy(itemsIds);
            _itemsSpawnService.SetSpawnItemsStrategy(levelItemsStrategy);
            SubscribeItemsListeners(levelItemsStrategy.GetItems());
        }

        public void OnPause()
        {
            _itemsSpawnService.DisableSpawn();
            
            foreach (var item in _items)
                item.PauseAnimations();
        }

        public void OnResume()
        {
            _itemsSpawnService.EnableSpawn();
            
            foreach (var item in _items)
                item.ResumeAnimations();
        }

        public float GetDropItemsDuration() =>
            _currentDropItemsDuration;

        public void UpdateItemsByTotalCatchAmount(int totalCatchItems) =>
            AdjustSpawnDelays(totalCatchItems);

        public void Clear()
        {
            _itemsSpawnDelay = _levelModel.DefaultItemsSpawnDelay;
            _currentDropItemsDuration = _levelModel.DefaultDropItemsDuration;
            
            _items.Clear();
            _itemsSpawnService.Clear();
        }

        private ILevelItemsStrategy DefineItemsStrategy(List<int> itemsIds)
        {
            ILevelItemsStrategy levelItemsStrategy;
            var selectedItem = _itemsService.GetSelectedItem();
            if (selectedItem == null)
            {
                levelItemsStrategy = new LevelMultipleItemsStrategy(_itemsService, itemsIds);
            }
            else
            {
                levelItemsStrategy = new LevelSingleItemStrategy(_itemsService, selectedItem.Id);
            }

            return levelItemsStrategy;
        }

        private void SubscribeItemsListeners(List<ItemModel> itemModels)
        {
            foreach (var itemModel in itemModels)
            {
                itemModel.OnCatchAction = OnCatchItem;
                itemModel.OnFailAction = OnFailItem;
                itemModel.RemoveItemAction = OnRemoveItem;
            }
        }

        private void SpawnItems()
        {
            _itemsSpawnService.UpdateSpawnDelay(_itemsSpawnDelay);
            _itemsSpawnService.Spawn();
        }

        private void OnCatchItem(ItemModel itemModel) =>
            OnCatchItemAction?.Invoke(itemModel);

        private void OnFailItem() =>
            OnFailItemAction?.Invoke();

        private void CalculateNewDelay(ref float currentDelay, int totalCatchItems, 
            float decreaseValue, float minimalValue)
        {
            if (totalCatchItems % _catchItemsToDecreaseSpawnDelay != 0)
                return;

            var decreasedDelay = currentDelay - decreaseValue;
            if (decreasedDelay >= minimalValue)
            {
                currentDelay = decreasedDelay;
            }
        }

        private void AdjustSpawnDelays(int totalCatchItems)
        {
            CalculateNewDelay(ref _currentDropItemsDuration, totalCatchItems, 
                _dropDurationDecreaseValue, 
                _levelModel.MinimalDropItemsDuration);

            CalculateNewDelay(ref _itemsSpawnDelay, totalCatchItems, 
                _spawnDelayDecreaseValue, 
                _levelModel.MinimalItemsSpawnDelay);

            _itemsSpawnService.UpdateSpawnDelay(_itemsSpawnDelay);
        }

        private void OnSpawnItem(ItemView itemView)
        {
            _items.Add(itemView);
            OnSpawnItemAction?.Invoke(itemView);
        }

        private void OnRemoveItem(ItemView itemView)
        {
            _items.Remove(itemView);
            _itemsSpawnService.RemoveItem(itemView);
        }

        private void AfterSetModel()
        {
            _itemsSpawnDelay = _levelModel.DefaultItemsSpawnDelay;
            _currentDropItemsDuration = _levelModel.DefaultDropItemsDuration;
            _catchItemsToDecreaseSpawnDelay = _levelModel.CatchItemsToDecreaseSpawnDelay;
            _spawnDelayDecreaseValue = _levelModel.SpawnDelayDecreaseValue;
            _dropDurationDecreaseValue = _levelModel.DropItemsDecreaseDurationValue;
        }
    }
}