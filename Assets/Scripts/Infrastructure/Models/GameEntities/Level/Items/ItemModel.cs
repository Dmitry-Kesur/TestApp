using System;
using Infrastructure.Data.Items;
using Infrastructure.Enums;
using Infrastructure.Views.GameEntities;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Level.Items
{
    public class ItemModel
    {
        private readonly ItemData _itemData;

        public Action<ItemModel> OnUnlockItemAction;
        public Action<ItemView> RemoveItemAction;

        protected ItemModel(ItemData itemData) =>
            _itemData = itemData;

        public void OnFail() =>
            OnFailAction?.Invoke();

        public void OnCatch() =>
            OnCatchAction?.Invoke(this);
        
        public int Id =>
            _itemData.Id;

        public int ScorePoints =>
            _itemData.ScorePoints;
        
        public float SpawnChance =>
            _itemData.SpawnChance;

        public bool Unlocked { get; set; }

        public virtual bool NeedDissolveEffect =>
            false;

        public virtual bool FailOnCatch =>
            false;

        public virtual bool FailOnReachedArea =>
            true;
        
        public ItemsType ItemType =>
            _itemData.ItemType;

        public Sprite GetSprite() =>
            _itemData.Sprite;

        public Action<ItemModel> OnCatchAction { get; set; }

        public Action OnFailAction { get; set; }

        public void OnRemoveItem(ItemView itemView) =>
            RemoveItemAction?.Invoke(itemView);
    }
}