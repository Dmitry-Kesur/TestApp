using System.Collections.Generic;
using Infrastructure.Services.Items;
using Infrastructure.Strategy;

namespace Infrastructure.Factories.Level
{
    public class ItemsStrategyFactory : IItemsStrategyFactory
    {
        private readonly IItemsService _itemsService;

        public ItemsStrategyFactory(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        public IItemsStrategy GetItemsStrategy(List<int> itemIds)
        {
            var selectedItem = _itemsService.GetSelectedItem();
            return selectedItem == null 
                ? new MultipleItemsStrategy(_itemsService, itemIds) 
                : new SingleItemStrategy(_itemsService, selectedItem.Id);
        }
    }
}