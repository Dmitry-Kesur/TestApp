using System;
using DefaultNamespace;
using UnityEngine;

namespace UI
{
    public class InterfaceView : MonoBehaviour
    {
        [SerializeField] private Transform interfaceContainerTransform;
        [SerializeField] private GameSessionView gameSessionView;
        private BaseWindow _activeWindow;
        private GameHandler _gameHandler;

        public void Init(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
            _gameHandler.interfaceModel.OnShowWindowAction = OnShowWindow;
            _gameHandler.interfaceModel.OnCloseWindowAction = OnCloseWindow;
            _gameHandler.interfaceModel.OnShowGameSessionInterfaceAction = OnShowGameSessionInterface;
            _gameHandler.interfaceModel.HideGameSessionInterfaceAction = OnHideGameSessionInterface;
        }

        private void OnShowWindow(BaseWindowModel windowModel)
        {
            _activeWindow = windowModel.GetWindowInstance();
            _activeWindow.transform.SetParent(interfaceContainerTransform, false);
        }

        private void OnCloseWindow()
        {
            if (_activeWindow == null)
            {
                Debug.LogException(new Exception("[InterfaceView]: activeWindow = null"));
            }
            
            Destroy(_activeWindow.gameObject);
            _activeWindow = null;
        }
        
        private void OnShowGameSessionInterface(GameSessionModel gameSessionModel)
        {
            gameSessionView.Init(gameSessionModel);
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