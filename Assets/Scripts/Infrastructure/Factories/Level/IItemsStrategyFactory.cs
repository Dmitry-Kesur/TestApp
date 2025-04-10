using System.Collections.Generic;
using Infrastructure.Strategy;

namespace Infrastructure.Factories.Level
{
    public interface IItemsStrategyFactory
    {
        IItemsStrategy GetItemsStrategy(List<int> itemsIds);
    }
}