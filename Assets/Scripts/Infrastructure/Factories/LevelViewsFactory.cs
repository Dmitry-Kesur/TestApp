using Infrastructure.Providers;
using Infrastructure.Views.GameEntities;
using UnityEngine;

namespace Infrastructure.Services
{
    public class LevelViewsFactory
    {
        private readonly SceneProvider _sceneProvider;

        public LevelViewsFactory(SceneProvider sceneProvider)
        {
            _sceneProvider = sceneProvider;
        }

        public LevelView CreateLevelView()
        {
            var levelPrefab = Resources.Load<LevelView>("Prefabs/Level/LevelView");
            var levelView = Object.Instantiate(levelPrefab, _sceneProvider.GameLevelLayer, false);
            return levelView;
        }
    }
}