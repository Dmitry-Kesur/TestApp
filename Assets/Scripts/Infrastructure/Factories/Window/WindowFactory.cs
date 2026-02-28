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
        private readonly Dictionary<WindowId, BaseWindow> _windowPrefabs = new();
        
        private readonly UIProvider _uiProvider;
        private readonly DiContainer _diContainer;

        private static readonly List<WindowId> WindowIds = new()
        {
            WindowId.MenuWindow, WindowId.PreloaderWindow, WindowId.SettingsWindow, WindowId.WinLevelWindow,
            WindowId.LoseLevelWindow, WindowId.PauseGameWindow, WindowId.SelectLevelWindow, WindowId.ShopWindow, WindowId.PremiumShopWindow, WindowId.AuthenticationWindow, WindowId.DailyBonusWindow, WindowId.BoosterActivationWindow, WindowId.LuckySpinWindow
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
            return windowView;
        }

        public BaseWindowController CreateController(BaseWindow windowView)
        {
            var controllerType = windowView.GetWindowControllerType();
            var windowController = (BaseWindowController) _diContainer.Instantiate(controllerType);
            return windowController;
        }
    }
}