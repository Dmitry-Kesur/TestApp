using System;
using System.Collections.Generic;
using Infrastructure.Controllers.Windows;
using Infrastructure.Enums;
using Infrastructure.Factories.Window;
using Infrastructure.Views.UI.Windows;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Services.Window
{
    public class WindowService : IWindowService
    {
        private readonly Dictionary<WindowId, BaseWindow> _windows = new();
        private readonly Dictionary<Type, BaseWindowController> _controllers = new();
        
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
            var windowController = GetWindowController(windowView) ?? _windowFactory.CreateController(windowView);

            windowController.SetWindowView(windowView);
            windowController.AfterWindowCreate();
            
            windowView.OnWindowShow();
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
        
        private BaseWindowController GetWindowController(BaseWindow windowView)
        {
            var controllerType = windowView.GetWindowControllerType();
            _controllers.TryGetValue(controllerType, out var windowController);
            return windowController;
        }
    }
}