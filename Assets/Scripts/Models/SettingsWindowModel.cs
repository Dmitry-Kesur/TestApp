using System;
using SimpleInjector;
using UI;
using UI.Windows;
using UnityEngine;

namespace DefaultNamespace
{
    public class SettingsWindowModel : BaseWindowModel
    {
        public Action OnReturnAction;
        private readonly ItemsService _itemsService;

        public SettingsWindowModel(Container container)
        {
            _itemsService = container.GetInstance<ItemsService>();
        }

        public ItemModel GetGameItem() => _itemsService.GetGameItem();

        public override BaseWindow GetWindowInstance()
        {
            if (windowInstance == null)
            {
                windowInstance = GameObject.Instantiate(Resources.Load<SettingsWindow>("Prefabs/UI/Windows/SettingsWindow"));
                windowInstance.Init(this);    
            }
            
            return windowInstance;
        }

        public void OnReturnButtonClick()
        {
            OnReturnAction?.Invoke();
        }
    }
}