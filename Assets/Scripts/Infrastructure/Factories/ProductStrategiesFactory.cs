using System.Collections.Generic;
using Infrastructure.Services;
using Infrastructure.Strategy;

namespace Infrastructure.Factories
{
    public class ProductStrategiesFactory : IProductStrategiesFactory
    {
        private readonly IItemsService _itemsService;

        public ProductStrategiesFactory(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        public List<IProductStrategy> CreateProductStrategies()
        {
            return new List<IProductStrategy> { new LevelItemProductsStrategy(_itemsService) };
        }
    }
}