using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Items;
using Infrastructure.Data.Preloader;
using Infrastructure.Enums;
using Infrastructure.Factories;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services.PlayerProgressUpdaters;

namespace Infrastructure.Services
{
    public class ItemsService : IItemsService, ILoadableService
    {
        private readonly List<ItemModel> _items = new();
        
        private readonly LocalAddressableService _addressableService;
        private readonly ItemsProgressUpdater _itemsProgressUpdater;
        private readonly IItemModelsFactory _itemModelsFactory;
        
        private List<ItemData> _itemsData;

        public ItemsService(LocalAddressableService localAddressableService, ItemsProgressUpdater itemsProgressUpdater, IItemModelsFactory itemModelsFactory)
        {
            _addressableService = localAddressableService;
            _itemsProgressUpdater = itemsProgressUpdater;
            _itemModelsFactory = itemModelsFactory;
        }

        public async Task Load()
        {
            _itemsData =
                await _addressableService.LoadScriptableCollectionFromGroupAsync<ItemData>(AddressableGroupNames
                    .LevelItemsGroup);
            CreateItems();
            UpdateUnlockedItems();
        }

        public LoadingStage LoadingStage => LoadingStage.LoadingGameItems;

        public ItemModel GetSelectedItem()
        {
            var selectedItemId = _itemsProgressUpdater.GetSelectedItemId();
            if (selectedItemId == 0)
                return null;

            var itemModel = GetItemById(selectedItemId);
            return itemModel;
        }

        public ItemModel GetItemById(int itemId)
        {
            var itemById = _items.Find(model => model.Id == itemId);
            return itemById;
        }

        public List<ItemModel> GetItemsByIds(List<int> itemIds)
        {
            List<ItemModel> items = new();

            foreach (var itemId in itemIds)
            {
                var itemModel = GetItemById(itemId);
                items.Add(itemModel);
            }

            return items;
        }

        public List<ItemModel> GetItemsByType(ItemsType itemsType)
        {
            var itemModels = _items.FindAll(model => model.ItemType == itemsType);
            return itemModels;
        }
        
        public List<ItemModel> GetUnlockedItems()
        {
            var unlockedItem = _items.FindAll(model => model.Unlocked);
            return unlockedItem;
        }

        private void CreateItems()
        {
            foreach (var itemData in _itemsData)
            {
                var itemModel = _itemModelsFactory.CreateItem(itemData);
                itemModel.OnUnlockItemAction = OnUnlockItem;
                _items.Add(itemModel);
            }
        }
        
        private void UpdateUnlockedItems()
        {
            var unlockedItemsIds = _itemsProgressUpdater.GetUnlockedItemIds();
            if (unlockedItemsIds is {Count: 0})
                return;

            foreach (var unlockedItemId in unlockedItemsIds)
            {
                var itemById = GetItemById(unlockedItemId);
                itemById.Unlocked = true;
            }
        }

        private void OnUnlockItem(ItemModel itemModel)
        {
            _itemsProgressUpdater.SetUnlockedItem(itemModel.Id);
            UpdateUnlockedItems();
        }
    }
}