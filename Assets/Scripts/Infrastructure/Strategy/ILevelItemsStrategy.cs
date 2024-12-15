using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level.Items;

namespace Infrastructure.Strategy
{
    public interface ILevelItemsStrategy
    {
        List<ItemModel> GetItems();
    }
}