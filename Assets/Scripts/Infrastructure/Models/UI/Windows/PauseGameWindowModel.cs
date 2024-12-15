using System;

namespace Infrastructure.Models.UI.Windows
{
    public class PauseGameWindowModel : BaseWindowModel
    {
        public Action OnBackToMenuButtonClickAction;
        public Action OnResumeGameButtonClickAction;
        public Action<bool> OnMuteSoundsStateChange;
        
        public void OnBackToMenuButtonClick()
        {
            OnBackToMenuButtonClickAction?.Invoke();
        }

        public void OnResumeGameButtonClick()
        {
            OnResumeGameButtonClickAction?.Invoke();
        }

        public void ChangeMuteSoundsState(bool muteSounds)
        {
            OnMuteSoundsStateChange?.Invoke(muteSounds);
        }
        
        public bool MuteSounds { get; set; }
    }
}