﻿using System;
using DefaultNamespace;
using UnityEngine;

namespace UI
{
    public class InterfaceView : MonoBehaviour
    {
        [SerializeField] private Transform interfaceContainerTransform;
        [SerializeField] private GameSessionView gameSessionView;
        private BaseWindow _activeWindow;
        private InterfaceService _interfaceService;

        public void Init(InterfaceService interfaceService)
        {
            _interfaceService = interfaceService;

            _interfaceService.OnShowWindowAction = OnShowWindow;
            _interfaceService.OnCloseWindowAction = OnCloseWindow;
            _interfaceService.OnShowGameSessionInterfaceAction = OnShowGameSessionInterface;
            _interfaceService.HideGameSessionInterfaceAction = OnHideGameSessionInterface;

            InitGameSession();
        }

        private void InitGameSession()
        {
            gameSessionView.Init(_interfaceService.gameSessionModel);
        }

        private void OnShowWindow(BaseWindowModel windowModel)
        {
            _activeWindow = windowModel.GetWindowInstance();
            _activeWindow.transform.SetParent(interfaceContainerTransform, false);
            _activeWindow.OnWindowShow();
        }

        private void OnCloseWindow()
        {
            if (_activeWindow == null)
            {
                Debug.LogException(new Exception("[InterfaceView]: activeWindow = null"));
            }

            _activeWindow.OnWindowHide();
            Destroy(_activeWindow.gameObject);
            _activeWindow = null;
        }

        private void OnShowGameSessionInterface()
        {
            gameSessionView.Show();
        }

        private void OnHideGameSessionInterface()
        {
            if (gameSessionView.isActiveAndEnabled)
            {
                gameSessionView.Hide();
            }
        }
    }
}