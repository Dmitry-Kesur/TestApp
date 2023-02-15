using System;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class ItemModel
    {
        public Action<int> OnCatchItemAction;
        private readonly int _id;
        private readonly int _scoreAmount;
        private bool _isSelected;
        private readonly Sprite _sprite;

        public ItemModel(int id, Sprite sprite)
        {
            _id = id;
            _scoreAmount = 2;
            _sprite = sprite;
        }

        public ItemView RenderItem()
        {
            var instance = GameObject.Instantiate(Resources.Load<ItemView>("Prefabs/UI/Views/ItemView"));
            instance.Init(this);
            return instance;
        }

        public int id => _id;

        public bool isSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public Sprite GetSprite() => _sprite;

        public void OnCatchItem()
        {
            OnCatchItemAction?.Invoke(_scoreAmount);
        }
    }
}