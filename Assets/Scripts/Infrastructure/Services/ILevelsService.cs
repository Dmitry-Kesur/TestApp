using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI;

namespace Infrastructure.Services
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