using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services;

namespace Infrastructure.Strategy
{
    public class BaseLevelItemsStrategy : ILevelItemsStrategy
    {
        protected readonly IItemsService ItemsService;

        protected BaseLevelItemsStrategy(IItemsService itemsService)
        {
            ItemsService = itemsService;
        }

        public virtual List<ItemModel> GetItems() =>
            ItemsService.GetItemsByType(ItemsType.Bomb);
    }
}