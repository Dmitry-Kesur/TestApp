using System;
using Infrastructure.Models.GameEntities.Level;

namespace Infrastructure.Models.UI.Windows
{
    public class LoseLevelWindowModel : BaseWindowModel
    {
        private readonly LevelSession _levelSession;
        
        public Action OnRestartButtonClickAction;
        public Action OnBackToMenuButtonClickAction;
        public Action OnContinueButtonClickAction;
        
        public LoseLevelWindowModel(LevelSession levelSession)
        {
            _levelSession = levelSession;
        }

        public int TotalScore =>
            _levelSession.TotalLevelScore;

        public void OnRestartButtonClick() =>
            OnRestartButtonClickAction?.Invoke();

        public void OnBackToMenuButtonClick() =>
            OnBackToMenuButtonClickAction?.Invoke();

        public void OnContinueButtonClick() =>
            OnContinueButtonClickAction?.Invoke();
    }
}