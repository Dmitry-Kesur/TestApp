using System;
using System.Collections.Generic;
using Infrastructure.Controllers.Windows;
using Infrastructure.Enums;
using Infrastructure.Providers.UI;
using Infrastructure.Views.UI.Windows;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.Window
{
    public class WindowFactory
    {
        private readonly Dictionary<Type, BaseWindowController> _controllers = new();
        private readonly Dictionary<WindowId, BaseWindow> _windowPrefabs = new();
        
        private readonly UIProvider _uiProvider;
        private readonly DiContainer _diContainer;

        private static readonly List<WindowId> WindowIds = new()
        {
            WindowId.MenuWindow, WindowId.PreloaderWindow, WindowId.SettingsWindow, WindowId.WinLevelWindow,
            WindowId.LoseLevelWindow, WindowId.PauseGameWindow, WindowId.SelectLevelWindow, WindowId.ShopWindow, WindowId.BoostersWindow, WindowId.AuthenticationWindow
        };

        public WindowFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _uiProvider = _diContainer.Resolve<UIProvider>();
            LoadWindowPrefabs();
        }

        private void LoadWindowPrefabs()
        {
            foreach (var windowId in WindowIds)
            {
                var windowPrefab = Resources.Load<BaseWindow>($"Prefabs/UI/Windows/{windowId.ToString()}");
                _windowPrefabs.Add(windowId, windowPrefab);
            }
        }

        public BaseWindow Create(WindowId windowId)
        {
            var windowPrefab = _windowPrefabs[windowId];
            var windowView = Object.Instantiate(windowPrefab, _uiProvider.WindowsLayer, false);

            GetWindowController(windowView);

            return windowView;
        }

        private void GetWindowController(BaseWindow windowView)
        {
            var controllerType = windowView.GetWindowControllerType();
            _controllers.TryGetValue(controllerType, out var windowController);

            if (windowController == null)
            {
                windowController = (BaseWindowController) _diContainer.Instantiate(controllerType);
                _controllers.Add(controllerType, windowController);
            }

            windowController?.OnWindowAdd(windowView);
        }
    }
}