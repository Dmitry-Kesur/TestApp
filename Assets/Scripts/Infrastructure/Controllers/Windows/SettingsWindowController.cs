using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Items;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using Infrastructure.Services.Sound;
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
            _soundService.OnChangeMuteSoundsAction += UpdateMuteSoundsState;

            _settingsWindowModel = new SettingsWindowModel()
            {
                OnReturnAction = OnReturnHandler,
                OnItemSelectAction = OnItemSelect,
                OnChangeMuteSoundsStateAction = OnChangeMuteSoundsState,
            };
            
            _settingsWindowModel.SetItems(_itemsService.GetItemsByType(ItemsType.Candy));
            _settingsWindowModel.SelectedItemId = _itemsProgressUpdater.GetSelectedItemId();
        }

        protected override BaseWindowModel GetModel() =>
            _settingsWindowModel;

        private void OnChangeMuteSoundsState(bool muteSounds)
        {
            _soundService.ChangeMuteSounds(muteSounds);
        }

        private void OnItemSelect(int itemId)
        {
            _itemsProgressUpdater.SetSelectedItemId(itemId);
        }

        private void OnReturnHandler()
        {
            _stateMachineService.TransitionTo(StateType.MenuState);
        }

        private void UpdateMuteSoundsState(bool muteSounds)
        {
            _settingsWindowModel.MuteSounds = muteSounds;
        }
    }
}