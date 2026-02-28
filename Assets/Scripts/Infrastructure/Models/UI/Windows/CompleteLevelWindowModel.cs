using System;
using Infrastructure.Models.GameEntities.Level;

namespace Infrastructure.Models.UI.Windows
{
    public class CompleteLevelWindowModel : BaseWindowModel
    {
        private LevelSession _winLevel;

        public Action OnNextLevelButtonClickAction;
        public Action OnMenuButtonClickAction;
        
        public bool CanStartNextLevel { get; set; }

        public int LevelScore { get; set; }

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