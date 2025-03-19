using System.Collections.Generic;
using Infrastructure.Strategy;

namespace Infrastructure.Factories.Purchase
{
    public interface IProductStrategiesFactory
    {
        List<IProductStrategy> CreateProductStrategies();
    }
}