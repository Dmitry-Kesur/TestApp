using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class InterfaceService
    {
        public Action<BaseWindowModel> OnShowWindowAction;
        public Action OnCloseWindowAction;
        public Action<GameSessionModel> OnShowGameSessionInterfaceAction;
        public Action HideGameSessionInterfaceAction;
        private BaseWindowModel _activeWindow;
        
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

        public void ShowGameSessionInterface(GameSessionModel gameSessionModel)
        {
            CloseActiveWindow();
            OnShowGameSessionInterfaceAction?.Invoke(gameSessionModel);
        }

        private void CloseActiveWindow()
        {
            if (_activeWindow == null)
            {
                Debug.LogException(new Exception("[InterfaceModel]: activeModel = null"));
            }

            _activeWindow = null;

            OnCloseWindowAction?.Invoke();
        }
    }
}