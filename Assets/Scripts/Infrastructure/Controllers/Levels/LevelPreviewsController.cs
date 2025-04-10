using System.Collections.Generic;
using Infrastructure.Data.Level;
using Infrastructure.Factories.Level;
using Infrastructure.Models.UI.Items;

namespace Infrastructure.Controllers.Levels
{
    public class LevelPreviewsController
    {
        private readonly List<LevelPreviewModel> _levelPreviews = new();
        
        private readonly ILevelModelsFactory _levelModelsFactory;

        public LevelPreviewsController(ILevelModelsFactory levelModelsFactory)
        {
            _levelModelsFactory = levelModelsFactory;
        }

        public List<LevelPreviewModel> GetPreviewsModels() =>
            _levelPreviews;

        public void CreatePreviews(List<LevelStaticData> levelsData)
        {
            foreach (var levelData in levelsData)
            {
                var previewModel = _levelModelsFactory.CreatePreviewModel(levelData);
                _levelPreviews.Add(previewModel);
            }
        }

        public void MarkPreviewAsComplete(int level)
        {
            var previewModel = GetPreviewByLevel(level);
            previewModel.IsActive = false;
            previewModel.IsComplete = true;
        }

        public void MarkPreviewAsActive(int level)
        {
            var previewModel = GetPreviewByLevel(level);
            previewModel.IsActive = true;
            previewModel.IsComplete = false;
        }

        private LevelPreviewModel GetPreviewByLevel(int level) =>
            _levelPreviews.Find(model => model.Level == level);
    }
}