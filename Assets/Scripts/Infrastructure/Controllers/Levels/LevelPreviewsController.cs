using System.Collections.Generic;
using Infrastructure.Data.Level;
using Infrastructure.Factories.Level;
using Infrastructure.Models.UI.Items;
using Infrastructure.Services.Log;

namespace Infrastructure.Controllers.Levels
{
    public class LevelPreviewsController
    {
        private readonly List<LevelPreviewModel> _levelPreviews = new();
        
        private readonly LevelFactory _levelFactory;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        public LevelPreviewsController(LevelFactory levelFactory, IExceptionLoggerService exceptionLoggerService)
        {
            _levelFactory = levelFactory;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public List<LevelPreviewModel> GetPreviewsModels() =>
            _levelPreviews;

        public void CreatePreviews(List<LevelStaticData> levelsData)
        {
            foreach (var levelData in levelsData)
            {
                var previewModel = _levelFactory.CreatePreviewModel(levelData);
                _levelPreviews.Add(previewModel);
            }
        }

        public void MarkPreviewAsComplete(int level)
        {
            var previewModel = GetPreviewByLevel(level);
            if (previewModel == null)
                return;
            
            previewModel.IsActive = false;
            previewModel.IsComplete = true;
        }

        public void MarkPreviewAsActive(int level)
        {
            var previewModel = GetPreviewByLevel(level);
            if (previewModel == null)
                return;
            
            previewModel.IsActive = true;
            previewModel.IsComplete = false;
        }

        private LevelPreviewModel GetPreviewByLevel(int level)
        {
            var levelPreview = _levelPreviews.Find(model => model.Level == level);
            if (levelPreview == null)
            {
                _exceptionLoggerService.LogError($"[level-preview] Preview not found for level {level}");
                return null;
            }
            
            return levelPreview;
        }
    }
}