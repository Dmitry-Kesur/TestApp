using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class InterfaceService
    {
        public Action<BaseWindowModel> OnShowWindowAction;
        public Action OnCloseWindowAction;
        public Action OnShowGameSessionInterfaceAction;
        public Action HideGameSessionInterfaceAction;
        private BaseWindowModel _activeWindow;
        private GameSessionModel _gameSessionModel;

        public GameSessionModel gameSessionModel => _gameSessionModel;
        
        public void SetGameSessionModel(GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
        }
        
        public void ShowWindow(BaseWindowModel windowModel)
        {
            HideGameSessionInterfaceAction?.Invoke();
            
            if (_activeWindow != null)
            {
                CloseActiveWindow();
            }
            
            _activeWindow = windowModel;
            OnShowWindowAction?.Invoke(_activeWindow);
        }

        public void ShowGameSessionInterface()
        {
            CloseActiveWindow();
            OnShowGameSessionInterfaceAction?.Invoke();
        }

        private void CloseActiveWindow()
        {
            if (_activeWindow == null)
            {
                Debug.LogException(new Exception("[InterfaceService]: activeModel = null"));
            }

            _activeWindow = null;

            OnCloseWindowAction?.Invoke();
        }
    }
}