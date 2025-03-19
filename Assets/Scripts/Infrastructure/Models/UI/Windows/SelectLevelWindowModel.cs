using System;
using System.Collections.Generic;
using Infrastructure.Models.UI.Items;

namespace Infrastructure.Models.UI.Windows
{
    public class SelectLevelWindowModel : BaseWindowModel
    {
        public Action<int> OnLevelSelectAction;
        public Action OnBackButtonClickAction;
        
        private List<LevelPreviewModel> _levelPreviews;

        public void SetLevelPreviews(List<LevelPreviewModel> levels)
        {
            _levelPreviews = levels;
        }

        public List<LevelPreviewModel> GetLevelPreviews() =>
            _levelPreviews;

        public void OnBackButtonClick()
        {
            OnBackButtonClickAction?.Invoke();
        }

        public void OnLevelSelect(int level)
        {
            OnLevelSelectAction?.Invoke(level);
        }
    }
}