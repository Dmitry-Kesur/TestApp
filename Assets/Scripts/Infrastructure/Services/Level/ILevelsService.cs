using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI.Items;

namespace Infrastructure.Services.Level
{
    public interface ILevelsService
    {
        bool ReachedMaxLevel { get; }
        List<LevelPreviewModel> GetPreviewsModels();
        LevelModel GetCurrentLevel();
        void Select(int level);
        void Start();
        void Stop();
        void Pause();
    }
}