using System.Collections.Generic;
using Infrastructure.Strategy;

namespace Infrastructure.Factories
{
    public interface IProductStrategiesFactory
    {
        List<IProductStrategy> CreateProductStrategies();
    }
}