using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI.Items;

namespace Infrastructure.Services.Level
{
    public interface ILevelsService
    {
        bool ReachedMaxLevel { get; }
        LevelResult LevelResult { get; }
        List<LevelPreviewModel> GetPreviewsModels();
        LevelModel GetCurrentLevel();
        void SelectLevel(int level);
        void Stop();
        void Pause();
        void Revive();
        void Restart();
        void Resume();
        void StartLevel();
    }
}