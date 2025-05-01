using Infrastructure.Views.GameEntities;
using UnityEngine;
using UnityEngine.Pool;

namespace Infrastructure.Factories.Level
{
    public class ItemViewsFactory : IItemViewsFactory
    {
        private readonly IObjectPool<ItemView> _itemsPool;

        public ItemViewsFactory()
        {
            _itemsPool = new ObjectPool<ItemView>(
                InstantiateItem, OnItemGet, OnItemRelease, OnItemDestroy, true, 15, 65);
        }
        
        public ItemView GetItem() =>
            _itemsPool.Get();
        
        public void ReleaseItem(ItemView itemView) =>
            _itemsPool.Release(itemView);

        public void Clear() =>
            _itemsPool.Clear();

        private void OnItemGet(ItemView view) =>
            view.gameObject.SetActive(true);

        private void OnItemRelease(ItemView view) => 
            view.gameObject.SetActive(false);

        private void OnItemDestroy(ItemView view) => 
            Object.Destroy(view.gameObject);

        private ItemView InstantiateItem()
        {
            var itemPrefab = Resources.Load<ItemView>("Prefabs/Level/ItemView");
            return Object.Instantiate(itemPrefab);
        }
    }
}