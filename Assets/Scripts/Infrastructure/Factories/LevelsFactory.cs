using Infrastructure.Data.Level;
using Infrastructure.Models.GameEntities.Level;
using Zenject;

namespace Infrastructure.Factories
{
    public class LevelsFactory : ILevelFactory
    {
        private readonly DiContainer _diContainer;

        public LevelsFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public LevelModel CreateModel(LevelStaticData levelData)
        {
            var levelModel = _diContainer.Instantiate<LevelModel>();
            levelModel.SetData(levelData);
            return levelModel;
        }
    }
}