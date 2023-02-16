using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class ItemModel
    {
        public Action<int> OnCatchItemAction;
        private int _activeSpriteIndex;
        private readonly int _rewardScoreAmount;
        private Dictionary<int, Sprite> _sprites;
        private Sprite _activeSprite;

        public ItemModel(Dictionary<int, Sprite> sprites, int activeSpriteIndex)
        {
            _sprites = sprites;
            _rewardScoreAmount = 2;

            ChangeActiveSprite(activeSpriteIndex);
        }

        public void ChangeActiveSprite(int spriteIndex)
        {
            _activeSpriteIndex = spriteIndex;
            _activeSprite = _sprites[_activeSpriteIndex];
        }

        public ItemView RenderItem()
        {
            var instance = GameObject.Instantiate(Resources.Load<ItemView>("Prefabs/UI/Views/ItemView"));
            instance.Init(this);
            return instance;
        }
        
        public int activeSpriteIndex => _activeSpriteIndex;

        public int spritesCount => _sprites.Count;
        
        public Sprite GetSpriteByIndex(int index) => _sprites[index];

        public Sprite GetActiveSprite() => _activeSprite;

        public void OnCatchItem()
        {
            OnCatchItemAction?.Invoke(_rewardScoreAmount);
        }
    }
}