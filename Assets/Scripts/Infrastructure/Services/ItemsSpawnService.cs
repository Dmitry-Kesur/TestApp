using System;
using Infrastructure.Factories;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Strategy;
using Infrastructure.Views.GameEntities;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Infrastructure.Services
{
    public class ItemsSpawnService : IItemsSpawnService, ITickable
    {
        private readonly IItemViewsFactory _itemsFactory;

        private float _itemsSpawnDelay;
        private float _spawnTimer;
        private bool _enableSpawn;

        private ILevelItemsStrategy _levelItemsStrategy;

        public ItemsSpawnService(IItemViewsFactory itemsFactory) =>
            _itemsFactory = itemsFactory;

        public void UpdateSpawnDelay(float spawnDelay) =>
            _itemsSpawnDelay = spawnDelay;

        public void SetSpawnItemsStrategy(ILevelItemsStrategy levelItemsStrategy) =>
            _levelItemsStrategy = levelItemsStrategy;

        public void Spawn()
        {
            SpawnItem();
            EnableSpawn();
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

        public void Clear()
        {
            _spawnTimer = 0;
            DisableSpawn();
            _itemsFactory.Clear();
        }

        public Action<ItemView> OnSpawnItemAction { get; set; }

        private ItemModel GetItemBySpawnChance()
        {
            float randomChanceValue = Random.Range(0f, 1f);
            float cumulativeChance = 0;

            var itemModels = _levelItemsStrategy.GetItems();

            foreach (var itemModel in itemModels)
            {
                cumulativeChance += itemModel.SpawnChance;
                if (randomChanceValue <= cumulativeChance)
                {
                    return itemModel;
                }
            }

            return GetItemBySpawnChance();
        }

        private void SpawnItem()
        {
            var itemModel = GetItemBySpawnChance();
            var itemView = _itemsFactory.GetItem();
            itemView.SetModel(itemModel);
            itemView.Draw();
            
            OnSpawnItemAction?.Invoke(itemView);
        }
    }
}