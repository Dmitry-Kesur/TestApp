using System;
using Infrastructure.Models.GameEntities.Level;

namespace Infrastructure.Models.UI.Windows
{
    public class LoseLevelWindowModel : BaseWindowModel
    {
        private readonly LevelModel _levelModel;
        
        public Action OnRestartButtonClickAction;
        public Action OnBackToMenuButtonClickAction;
        
        public LoseLevelWindowModel(LevelModel levelModel)
        {
            _levelModel = levelModel;
        }

        public int TotalScore =>
            _levelModel.TotalLevelScore;

        public int TotalFailItems =>
            _levelModel.TotalFailItems;

        public void OnRestartButtonClick() =>
            OnRestartButtonClickAction?.Invoke();

        public void OnBackToMenuButtonClick() =>
            OnBackToMenuButtonClickAction?.Invoke();
    }
}