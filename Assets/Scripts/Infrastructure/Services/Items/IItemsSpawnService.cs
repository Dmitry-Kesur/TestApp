using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Views.GameEntities;

namespace Infrastructure.Services.Items
{
    public interface IItemsSpawnService
    {
        void UpdateSpawnDelay(float spawnDelay);
        void SetItemModels(List<ItemModel> itemModels);
        void Spawn();
        void RemoveItem(ItemView itemView);
        void DisableSpawn();
        void EnableSpawn();
        void Clear();
        Action<ItemView> OnSpawnItemAction { get; set; }
    }
}