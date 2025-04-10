using System.Collections.Generic;
using Infrastructure.Strategy;

namespace Infrastructure.Factories.Purchase
{
    public interface IShopProductStrategiesFactory
    {
        List<IProductStrategy> CreateProductStrategies();
    }
}