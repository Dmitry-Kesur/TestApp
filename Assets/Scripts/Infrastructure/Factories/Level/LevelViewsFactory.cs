using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Providers.Scene;
using Infrastructure.Views.GameEntities;
using UnityEngine;

namespace Infrastructure.Factories.Level
{
    public class LevelViewsFactory
    {
        private readonly SceneProvider _sceneProvider;

        private LevelView _levelView;

        public LevelViewsFactory(SceneProvider sceneProvider)
        {
            _sceneProvider = sceneProvider;
        }

        public void CreateLevelView(LevelModel levelModel)
        {
            var levelPrefab = Resources.Load<LevelView>("Prefabs/Level/LevelView");
            _levelView = Object.Instantiate(levelPrefab, _sceneProvider.GameLevelLayer, false);
            _levelView.SetModel(levelModel);
        }

        public void DestroyLevelView() =>
            GameObject.Destroy(_levelView.gameObject);
    }
}