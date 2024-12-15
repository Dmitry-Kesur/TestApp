using System;
using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Views.GameEntities;

namespace Infrastructure.Services
{
    public interface IItemsService
    {
        List<ItemModel> GetItemsByIds(List<int> itemIds);
        List<ItemModel> GetItemsByType(ItemsType itemsType);
        List<ItemModel> GetUnlockedItems();
        ItemModel GetSelectedItem();
        ItemModel GetItemById(int itemId);
    }
}