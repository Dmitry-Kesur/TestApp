using System;
using Infrastructure.Strategy;
using Infrastructure.Views.GameEntities;

namespace Infrastructure.Services
{
    public interface IItemsSpawnService
    {
        void UpdateSpawnDelay(float spawnDelay);
        void SetSpawnItemsStrategy(ILevelItemsStrategy levelItemsStrategy);
        void Spawn();
        void RemoveItem(ItemView itemView);
        void DisableSpawn();
        void EnableSpawn();
        void Clear();
        Action<ItemView> OnSpawnItemAction { get; set; }
    }
}