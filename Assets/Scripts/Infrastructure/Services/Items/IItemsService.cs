using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Level.Items;

namespace Infrastructure.Services.Items
{
    public interface IItemsService
    {
        List<ItemModel> GetItemsByIds(List<int> itemIds);
        List<ItemModel> GetItemsByType(ItemsType itemsType);
        List<ItemModel> GetUnlockedItems();
        ItemModel GetSelectedItem();
        ItemModel GetItemById(int itemId);
        void Initialize();
    }
}