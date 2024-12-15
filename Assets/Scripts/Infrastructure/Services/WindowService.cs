using System.Collections.Generic;
using Infrastructure.Constants;
using Infrastructure.Enums;
using Infrastructure.Factories;
using Infrastructure.Views.UI.Windows;
using UnityEngine;

namespace Infrastructure.Services
{
    public class WindowService : IWindowService
    {
        private readonly Dictionary<WindowId, BaseWindow> _windows = new();
        private readonly WindowFactory _windowFactory;
        private WindowId _activeWindowId;

        public WindowService(WindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void ShowWindow(WindowId windowId)
        {
            HideActiveWindow();
            
            var windowView = _windowFactory.Create(windowId);
            windowView.OnShowWindow();
            windowView.Init();
            _windows.TryAdd(windowId, windowView);
            _activeWindowId = windowId;
        }

        public void HideWindow(WindowId windowId)
        {
            _windows.TryGetValue(windowId, out var windowView);
            if (windowView != null)
            {
                Object.Destroy(windowView.gameObject);
            }

            _windows.Remove(windowId);
            _activeWindowId = WindowId.None;
        }

        public void HideActiveWindow()
        {
            if (_activeWindowId == WindowId.None)
                return;

            HideWindow(_activeWindowId);
        }
    }
}