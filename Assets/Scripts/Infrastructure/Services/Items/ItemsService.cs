using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Items;
using Infrastructure.Data.Preloader;
using Infrastructure.Enums;
using Infrastructure.Factories.Level;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress;

namespace Infrastructure.Services.Items
{
    public class ItemsService : IItemsService, ILoadableService, IBootstrapTarget
    {
        private readonly List<ItemModel> _items = new();

        private readonly LocalAddressableService _addressableService;
        private readonly SaveLoadProgressService _progressService;
        private readonly ItemModelsFactory _itemModelsFactory;

        private List<ItemData> _itemsData;

        public ItemsService(LocalAddressableService localAddressableService, SaveLoadProgressService progressService,
            ItemModelsFactory itemModelsFactory)
        {
            _addressableService = localAddressableService;
            _progressService = progressService;
            _itemModelsFactory = itemModelsFactory;
        }

        public async Task Load()
        {
            _itemsData =
                await _addressableService.LoadScriptableCollectionFromGroupAsync<ItemData>(AddressableGroupNames
                    .LevelItemsGroup);
        }

        public int InitializationOrder => 2;

        public void Initialize()
        {
            CreateItems();
            UpdateUnlockedItems();
        }

        public LoadingStage LoadingStage => LoadingStage.LoadingItems;
        
        public void UnlockItem(int itemId)
        {
            var model = GetItemById(itemId);
            _progressService.Write(progress => progress.AddUnlockedLevelItem(itemId));
            model.Unlocked = true;
        }

        public ItemModel GetSelectedItem()
        {
            var selectedItemId = _progressService.Read(progress => progress.SelectedItemId);
            if (selectedItemId == 0)
                return null;

            var itemModel = GetItemById(selectedItemId);
            return itemModel;
        }

        public ItemModel GetItemById(int itemId) =>
            _items.Find(model => model.Id == itemId);

        public List<ItemModel> GetItemsByIds(List<int> itemIds)
        {
            List<ItemModel> items = new();

            foreach (var itemId in itemIds)
            {
                var itemModel = GetItemById(itemId);
                if (itemModel != null)
                    items.Add(itemModel);
            }

            return items;
        }

        public List<ItemModel> GetItemsByType(ItemsType itemsType) =>
            _items.FindAll(model => model.ItemType == itemsType);

        public List<ItemModel> GetUnlockedItems() =>
            _items.FindAll(model => model.Unlocked);

        private void CreateItems()
        {
            foreach (var itemData in _itemsData)
            {
                var itemModel = _itemModelsFactory.CreateItem(itemData);
                _items.Add(itemModel);
            }
        }

        private void UpdateUnlockedItems()
        {
            var unlockedItemsIds = _progressService.Read(progress => progress.UnlockedLevelItemIds);
            if (unlockedItemsIds is { Count: 0 })
                return;

            foreach (var unlockedItemId in unlockedItemsIds)
            {
                var itemModel = GetItemById(unlockedItemId);
                if (itemModel != null)
                    itemModel.Unlocked = true;
            }
        }
    }
}