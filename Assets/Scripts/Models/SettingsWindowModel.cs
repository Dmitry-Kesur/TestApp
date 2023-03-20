using SimpleInjector;
using UI;
using UI.Windows;
using UnityEngine;

namespace DefaultNamespace
{
    public class SettingsWindowModel : BaseWindowModel
    {
        private readonly StateMachine _stateMachine;
        private readonly ItemsService _itemsService;

        public SettingsWindowModel(Container dependencyInjectionContainer)
        {
            _stateMachine = dependencyInjectionContainer.GetInstance<StateMachine>();
            _itemsService = dependencyInjectionContainer.GetInstance<ItemsService>();
        }

        public ItemModel GetGameItem() => _itemsService.GetGameItem();

        public override BaseWindow GetWindowInstance()
        {
            var instance = GameObject.Instantiate(Resources.Load<SettingsWindow>("Prefabs/UI/Windows/SettingsWindow"));
            instance.Init(this);
            return instance;
        }

        public void OnReturnButtonClick()
        {
            _stateMachine.SetState(StateName.MenuState);
        }
    }
}