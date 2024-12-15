using System;
using Infrastructure.Models.GameEntities.Level;

namespace Infrastructure.Models.UI.Windows
{
    public class CompleteLevelWindowModel : BaseWindowModel
    {
        private LevelModel _winLevel;

        public Action OnNextLevelButtonClickAction;
        public Action OnMenuButtonClickAction;

        public void SetWinLevel(LevelModel level)
        {
            _winLevel = level;
        }
        
        public bool CanStartNextLevel { get; set; }

        public int LevelScore =>
            _winLevel.TotalLevelScore;

        public void OnNextLevelButtonClick()
        {
            OnNextLevelButtonClickAction?.Invoke();
        }

        public void OnMenuButtonClick()
        {
            OnMenuButtonClickAction?.Invoke();
        }
    }
}