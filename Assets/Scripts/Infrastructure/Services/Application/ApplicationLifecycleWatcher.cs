using System;

namespace Infrastructure.Services.Application
{
    public class ApplicationLifecycleWatcher : IDisposable
    {
        public Action OnFocusInAction;
        public Action OnFocusOutAction;
        public Action OnQuitAction;
        
        public ApplicationLifecycleWatcher()
        {
            UnityEngine.Application.quitting += OnQuit;
            UnityEngine.Application.focusChanged += OnApplicationFocusChanged;
        }

        public void Dispose()
        {
            UnityEngine.Application.quitting -= OnQuit;
            UnityEngine.Application.focusChanged -= OnApplicationFocusChanged;
        }

        private void OnApplicationFocusChanged(bool focused)
        {
            if (focused)
                OnFocusInAction?.Invoke();
            else
                OnFocusOutAction?.Invoke();
        }

        private void OnQuit() =>
            OnQuitAction?.Invoke();
    }
}