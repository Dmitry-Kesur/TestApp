using System;

namespace Infrastructure.Models.UI.Windows
{
    public class PreloaderWindowModel : BaseWindowModel
    {
        public Action<float, string> OnUpdateLoadingProgressAction;
        public Action StartGameAction;
        
        public void OnUpdateLoadingProgress(float progress, string stageText)
        {
            OnUpdateLoadingProgressAction?.Invoke(progress, stageText);
        }

        public void StartGame()
        {
            StartGameAction?.Invoke();
        }
    }
}