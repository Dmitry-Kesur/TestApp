using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.PlayerProgressUpdaters;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class SettingsWindowController : BaseWindowController<SettingsWindow>
    {
        private readonly IItemsService _itemsService;
        private readonly StateMachineService _stateMachineService;
        private readonly ItemsProgressUpdater _itemsProgressUpdater;
        private readonly SettingsWindowModel _settingsWindowModel;
        private readonly ISoundService _soundService;

        public SettingsWindowController(IItemsService itemsService, StateMachineService stateMachineService,
            ItemsProgressUpdater itemsProgressUpdater, ISoundService soundService)
        {
            _itemsService = itemsService;
            _stateMachineService = stateMachineService;
            _itemsProgressUpdater = itemsProgressUpdater;
            _soundService = soundService;

            _settingsWindowModel = new SettingsWindowModel()
            {
                OnReturnAction = OnReturnHandler,
                OnItemSelectAction = OnItemSelect,
                OnChangeMuteSoundsStateAction = OnChangeMuteSoundsState,
            };
            
            _settingsWindowModel.SetItems(_itemsService.GetItemsByType(ItemsType.Candy));
            _settingsWindowModel.SelectedItemId = _itemsProgressUpdater.GetSelectedItemId();
        }

        private void OnChangeMuteSoundsState(bool muteSounds)
        {
            _soundService.ChangeMuteSounds(muteSounds);
        }

        public override void OnWindowAdd(BaseWindow view)
        {
            base.OnWindowAdd(view);
            _settingsWindowModel.MuteSounds = _soundService.MuteSounds;
            windowView.SetModel(_settingsWindowModel);
        }

        private void OnItemSelect(int itemId)
        {
            _itemsProgressUpdater.SetSelectedItemId(itemId);
        }

        private void OnReturnHandler()
        {
            _stateMachineService.TransitionTo(StateType.MenuState);
        }
    }
}