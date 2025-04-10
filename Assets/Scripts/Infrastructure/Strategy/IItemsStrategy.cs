using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level.Items;

namespace Infrastructure.Strategy
{
    public interface IItemsStrategy
    {
        List<ItemModel> GetItems();
    }
}