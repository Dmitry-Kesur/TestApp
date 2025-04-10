﻿using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services.Items;

namespace Infrastructure.Strategy
{
    public class MultipleItemsStrategy : BaseItemsStrategy
    {
        private readonly List<int> _itemIds;
        
        public MultipleItemsStrategy(IItemsService itemsService, List<int> itemIds) : base(itemsService)
        {
            _itemIds = itemIds;
        }

        public override List<ItemModel> GetItems()
        {
            List<ItemModel> levelItems = base.GetItems();
            
            var unlockedItems = ItemsService.GetUnlockedItems();
            var itemsByIds = ItemsService.GetItemsByIds(_itemIds);
            
            levelItems.AddRange(unlockedItems);
            levelItems.AddRange(itemsByIds);
            return levelItems;
        }
    }
}