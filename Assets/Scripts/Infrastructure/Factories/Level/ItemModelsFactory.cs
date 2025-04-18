﻿using Infrastructure.Data.Items;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Level.Items;

namespace Infrastructure.Factories.Level
{
    public class ItemModelsFactory
    {
        public ItemModel CreateItem(ItemData itemData)
        {
            if (itemData.ItemType == ItemsType.Candy)
            {
                return new CandyItemModel(itemData);
            }

            if (itemData.ItemType == ItemsType.Bomb)
            {
                return new BombItemModel(itemData);
            }

            return null;
        }
    }
}