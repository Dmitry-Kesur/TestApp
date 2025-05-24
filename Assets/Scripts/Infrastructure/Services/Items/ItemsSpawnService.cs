using System;
using System.Collections.Generic;
using Infrastructure.Factories.Level;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services.Log;
using Infrastructure.Views.GameEntities;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.Services.Items
{
    public class ItemsSpawnService : IItemsSpawnService, ITickable
    {
        private readonly IItemViewsFactory _itemsFactory;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        private float _itemsSpawnDelay;
        private float _spawnTimer;
        private bool _enableSpawn;

        private List<ItemModel> _itemModels;

        public ItemsSpawnService(IItemViewsFactory itemsFactory, IExceptionLoggerService exceptionLoggerService)
        {
            _itemsFactory = itemsFactory;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public void UpdateSpawnDelay(float spawnDelay) =>
            _itemsSpawnDelay = spawnDelay;

        public void SetItemModels(List<ItemModel> itemModels) =>
            _itemModels = itemModels;

        public void StartSpawnCycle()
        {
            EnableSpawn();
            SpawnItem();
        }

        public void RemoveItem(ItemView itemView) =>
            _itemsFactory.ReleaseItem(itemView);

        public void Tick()
        {
            if (_enableSpawn)
            {
                _spawnTimer += Time.deltaTime;

                if (_spawnTimer >= _itemsSpawnDelay)
                {
                    _spawnTimer = 0;
                    SpawnItem();
                }
            }
        }

        public void EnableSpawn() =>
            _enableSpawn = true;

        public void DisableSpawn() =>
            _enableSpawn = false;

        public bool Enabled => _enableSpawn;
        
        public void Clear()
        {
            _spawnTimer = 0;
            DisableSpawn();
            _itemsFactory.Clear();
        }

        public Action<ItemView> OnSpawnItemAction { get; set; }

        private ItemModel GetItemBySpawnChance()
        {
            if (_itemModels == null)
            {
                _exceptionLoggerService.LogError($"[ItemsSpawnService] Item models not set for spawn");
                return null;
            }
            
            float randomChanceValue = Random.Range(0f, 1f);
            float cumulativeChance = 0;

            foreach (var itemModel in _itemModels)
            {
                cumulativeChance += itemModel.SpawnChance;
                if (randomChanceValue <= cumulativeChance)
                {
                    return itemModel;
                }
            }

            return _itemModels[Random.Range(0, _itemModels.Count)];
        }

        private void SpawnItem()
        {
            var itemModel = GetItemBySpawnChance();
            
            var itemView = _itemsFactory.GetItem();
            itemView.SetModel(itemModel);
            itemView.OnSpawn();
            
            OnSpawnItemAction?.Invoke(itemView);
        }
    }
}