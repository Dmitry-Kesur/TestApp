﻿using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services;

namespace Infrastructure.Strategy
{
    public class LevelSingleItemStrategy : BaseLevelItemsStrategy
    {
        private readonly int _itemId;

        public LevelSingleItemStrategy(IItemsService itemsService, int itemId) : base(itemsService)
        {
            _itemId = itemId;
        }

        public override List<ItemModel> GetItems()
        {
            List<ItemModel> itemModels = base.GetItems();
            var itemsByIds = ItemsService.GetItemsByIds(new List<int> {_itemId});
            itemModels.AddRange(itemsByIds);
            return itemModels;
        }
    }
}