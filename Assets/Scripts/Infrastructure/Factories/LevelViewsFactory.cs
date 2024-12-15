using Infrastructure.Factories;
using Infrastructure.Providers;
using Infrastructure.Views.GameEntities;
using UnityEngine;
using UnityEngine.Pool;

namespace Infrastructure.Services
{
    public class LevelViewsFactory : ILevelViewsFactory
    {
        private readonly IObjectPool<ItemView> _itemsPool;

        private readonly SceneProvider _sceneProvider;

        private string CurentItemId;

        public LevelViewsFactory(SceneProvider sceneProvider)
        {
            _sceneProvider = sceneProvider;

            _itemsPool = new ObjectPool<ItemView>(
                InstantiateItemView, OnViewGet, OnViewRelease, OnViewDestroy, true, 15, 65);
        }

        public ItemView GetItem() =>
            _itemsPool.Get();

        public LevelView CreateLevelView()
        {
            var levelPrefab = Resources.Load<LevelView>("Prefabs/Level/LevelView");
            var levelView = Object.Instantiate(levelPrefab, _sceneProvider.GameLevelLayer, false);
            return levelView;
        }

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