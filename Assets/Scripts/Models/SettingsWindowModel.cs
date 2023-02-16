using UI;
using UI.Windows;
using UnityEngine;

namespace DefaultNamespace
{
    public class SettingsWindowModel : BaseWindowModel
    {
        private readonly GameHandler _gameHandler;

        public SettingsWindowModel(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }

        public ItemModel GetGameItem() => _gameHandler.itemsHandler.GetGameItem();

        public override BaseWindow GetWindowInstance()
        {
            var instance = GameObject.Instantiate(Resources.Load<SettingsWindow>("Prefabs/UI/Windows/SettingsWindow"));
            instance.Init(this);
            return instance;
        }

        public void OnReturnButtonClick()
        {
            _gameHandler.stateMachine.SetState(StateName.MenuState);
        }
    }
}