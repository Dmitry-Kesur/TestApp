using System.Collections.Generic;
using Infrastructure.Services.Items;
using Infrastructure.Strategy;

namespace Infrastructure.Factories.Purchase
{
    public class ShopProductStrategiesFactory : IShopProductStrategiesFactory
    {
        private readonly IItemsService _itemsService;

        public ShopProductStrategiesFactory(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        public List<IProductStrategy> CreateProductStrategies()
        {
            return new List<IProductStrategy> { new LevelItemProductsStrategy(_itemsService) };
        }
    }
}