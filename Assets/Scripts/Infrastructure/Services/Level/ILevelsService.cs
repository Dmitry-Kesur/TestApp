using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI.Items;

namespace Infrastructure.Services.Level
{
    public interface ILevelsService
    {
        bool ReachedMaxLevel { get; }
        bool LevelStarted { get; }
        List<LevelPreviewModel> GetPreviewsModels();
        LevelModel GetCurrentLevel();
        void SetCurrentLevel(int level);
        void Start();
        void Stop();
        void Pause();
        void Resume();
        void Initialize();
    }
}