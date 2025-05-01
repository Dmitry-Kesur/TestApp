using System;
using UnityEngine;

namespace Infrastructure.Services
{
    public class ApplicationFocusWatcher : IDisposable
    {
        public Action OnFocusIn;
        public Action OnFocusOut;
        
        public ApplicationFocusWatcher()
        {
            Application.focusChanged += OnApplicationFocusChanged;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnApplicationFocusChanged;
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