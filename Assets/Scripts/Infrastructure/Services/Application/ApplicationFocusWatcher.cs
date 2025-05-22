using System;

namespace Infrastructure.Services.Application
{
    public class ApplicationFocusWatcher : IDisposable
    {
        public Action OnFocusIn;
        public Action OnFocusOut;
        
        public ApplicationFocusWatcher()
        {
            UnityEngine.Application.focusChanged += OnApplicationFocusChanged;
        }

        public void Dispose()
        {
            UnityEngine.Application.focusChanged -= OnApplicationFocusChanged;
        }

        private void OnApplicationFocusChanged(bool focused)
        {
            if (focused)
                OnFocusIn?.Invoke();
            else
                OnFocusOut?.Invoke();
        }
    }
}