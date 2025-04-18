﻿using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services.Items;

namespace Infrastructure.Strategy
{
    public class BaseItemsStrategy : IItemsStrategy
    {
        protected readonly IItemsService ItemsService;

        protected BaseItemsStrategy(IItemsService itemsService)
        {
            ItemsService = itemsService;
        }

        public virtual List<ItemModel> GetItems() =>
            ItemsService.GetItemsByType(ItemsType.Bomb);
    }
}