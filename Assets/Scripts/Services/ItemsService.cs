using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ItemsService
    {
        private ItemModel _itemModel;
        private readonly GameDataController _gameDataController;

        public ItemsService(GameDataController gameDataController)
        {
            _gameDataController = gameDataController;
        }

        public void Init()
        {
            var sprites = LocalAssetBundleLoader.LoadSpritesBundle(GameAssetBundles.ItemSprites);

            Dictionary<int, Sprite> spritesDictionary = new Dictionary<int, Sprite>();
            for (int i = 0; i < sprites.Length; i++)
            {
                var sprite = sprites[i];
                spritesDictionary.Add(i, sprite);   
            }
            
            var randomSpriteIndex = Random.Range(0, spritesDictionary.Count);
            _itemModel = new ItemModel(spritesDictionary, randomSpriteIndex)
            {
                OnCatchItemAction = OnCatchItemHandle
            };
        }

        public ItemModel GetGameItem() => _itemModel;

        private void OnCatchItemHandle(int rewardScoreAmount)
        {
            _gameDataController.SetScore(rewardScoreAmount);
        }
    }
}