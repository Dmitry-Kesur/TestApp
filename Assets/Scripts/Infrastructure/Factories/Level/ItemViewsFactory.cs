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
                InstantiateItemView, OnViewGet, OnViewRelease, OnViewDestroy, true, 15, 65);
        }
        
        public ItemView GetItem() =>
            _itemsPool.Get();
        
        public void ReleaseItem(ItemView itemView) =>
            _itemsPool.Release(itemView);

        public void Clear() =>
            _itemsPool.Clear();

        private void OnViewGet(ItemView view) =>
            view.gameObject.SetActive(true);

        private void OnViewRelease(ItemView view) => 
            view.gameObject.SetActive(false);

        private void OnViewDestroy(ItemView view) => 
            Object.Destroy(view.gameObject);

        private ItemView InstantiateItemView()
        {
            var itemPrefab = Resources.Load<ItemView>("Prefabs/Level/ItemView");
            return Object.Instantiate(itemPrefab);
        }
    }
}