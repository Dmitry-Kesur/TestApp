using System;
using Infrastructure.Models.GameEntities.Boosters;
using Infrastructure.Services;
using Infrastructure.Services.Level;

namespace Infrastructure.Models.UI.HUD
{
    public class HudModel
    {
        public Action OnPauseGameButtonClickAction;

        private readonly ILevelsService _levelsService;

        public HudModel(ILevelsService levelsService)
        {
            _levelsService = levelsService;
        }

        public int ActiveLevelFailItems
        {
            get
            {
                var activeLevel = _levelsService.GetCurrentLevel();
                return activeLevel.TotalFailItems;
            }
        }

        public int ActiveLevelScore
        {
            get
            {
                var activeLevel = _levelsService.GetCurrentLevel();
                return activeLevel.TotalLevelScore;
            }
        }

        public BoosterModel ActiveBoosterModel { get; set; }

        public void OnPauseGameButtonClick()
        {
            OnPauseGameButtonClickAction?.Invoke();
        }
    }
}