using System.Collections.Generic;
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

        public List<ItemModel> GetItems() => _gameHandler.itemsHandler.GetItems();

        public ItemModel selectedItem => _gameHandler.itemsHandler.GetSelectedItem();

        public override BaseWindow GetWindowInstance()
        {
            var instance = GameObject.Instantiate(Resources.Load<SettingsWindow>("Prefabs/UI/Windows/SettingsWindow"));
            instance.Init(this);
            return instance;
        }

        public void SelectItem(int selectedItemId)
        {
            _gameHandler.itemsHandler.SetSelectedItem(selectedItemId);
        }

        public void OnReturnButtonClick()
        {
            _gameHandler.stateMachine.SetState(StateName.MenuState);
        }
    }
}