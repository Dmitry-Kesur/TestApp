using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ItemsHandler
    {
        private readonly List<ItemModel> _itemsModels;
        private ItemModel _selectedItem;
        private readonly GameHandler _gameHandler;

        public ItemsHandler(GameHandler gameHandler)
        {
            _itemsModels = new List<ItemModel>();
            _gameHandler = gameHandler;
        }

        public void Init()
        {
            var sprites = LocalAssetBundleLoader.LoadSpritesBundle(GameAssetBundles.ItemSprites);
            
            for (int i = 0; i < sprites.Length; i++)
            {
                var sprite = sprites[i];
                var model = new ItemModel(i, sprite);
                _itemsModels.Add(model);
            }

            var randomIndex = Random.Range(0, _itemsModels.Count);
            
            _selectedItem = _itemsModels[randomIndex];
            _selectedItem.OnCatchItemAction = OnCatchItemHandle;
            _selectedItem.isSelected = true;
        }

        public List<ItemModel> GetItems() => _itemsModels;

        public ItemModel GetSelectedItem() => _itemsModels.Find(model => model.isSelected);

        public void SetSelectedItem(int selectedItemId)
        {
            DeselectItems();

            var selectedItemModel = _itemsModels.Find(model => model.id == selectedItemId);
            selectedItemModel.isSelected = true;
        }

        private void DeselectItems()
        {
            foreach (var itemModel in _itemsModels)
            {
                itemModel.isSelected = false;
            }
        }

        private void OnCatchItemHandle(int rewardScoreAmount)
        {
            _gameHandler.scoreModel.SetScore(rewardScoreAmount);
        }
    }
}