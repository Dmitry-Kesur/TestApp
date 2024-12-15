using Infrastructure.Data.Items;
using Infrastructure.Models.GameEntities.Level.Items;

namespace Infrastructure.Factories
{
    public interface IItemModelsFactory
    {
        ItemModel CreateItem(ItemData itemData);
    }
}