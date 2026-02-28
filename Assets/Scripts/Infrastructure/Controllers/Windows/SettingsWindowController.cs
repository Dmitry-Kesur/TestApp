using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.Items;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Sound;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class SettingsWindowController : BaseWindowController<SettingsWindow>
    {
        private readonly IItemsService _itemsService;
        private readonly StateMachineService _stateMachineService;
        private readonly SaveLoadProgressService _saveLoadProgressService;
        private readonly SettingsWindowModel _settingsWindowModel;
        private readonly ISoundService _soundService;

        public SettingsWindowController(IItemsService itemsService, StateMachineService stateMachineService,
            SaveLoadProgressService saveLoadProgressService, ISoundService soundService)
        {
            _itemsService = itemsService;
            _stateMachineService = stateMachineService;
            _saveLoadProgressService = saveLoadProgressService;
            _soundService = soundService;
            _soundService.OnChangeMuteSoundsAction += UpdateMuteSoundsState;

            _settingsWindowModel = new SettingsWindowModel()
            {
                OnReturnAction = OnReturnHandler,
                OnItemSelectAction = OnItemSelect,
                OnChangeMuteSoundsStateAction = OnChangeMuteSoundsState,
            };

            _settingsWindowModel.SetItems(_itemsService.GetItemsByType(ItemsType.Candy));
            _settingsWindowModel.SelectedItemId = _saveLoadProgressService.Read(progress => progress.SelectedItemId);
        }

        protected override BaseWindowModel GetModel() =>
            _settingsWindowModel;

        private void OnChangeMuteSoundsState(bool muteSounds)
        {
            _soundService.ChangeMuteSounds(muteSounds);
        }

        private void OnItemSelect(int itemId)
        {
            _saveLoadProgressService.Write(progress => progress.SelectedItemId = itemId);
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