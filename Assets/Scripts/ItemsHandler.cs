using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ItemsHandler
    {
        private ItemModel _itemModel;
        private readonly GameHandler _gameHandler;

        public ItemsHandler(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
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
            _gameHandler.gameDataController.SetScore(rewardScoreAmount);
        }
    }
}