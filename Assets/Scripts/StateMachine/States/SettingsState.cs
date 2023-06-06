using SimpleInjector;

namespace DefaultNamespace.States
{
    public class SettingsState : BaseState
    {
        private readonly SettingsWindowModel _settingsWindowModel;
        private readonly InterfaceService _interfaceService;
        private BaseState _previousState;

        public SettingsState(StateType stateType, Container container) : base(stateType, container)
        {
            _settingsWindowModel = new SettingsWindowModel(container)
            {
                OnReturnAction = OnReturnHandler
            };

            _interfaceService = container.GetInstance<InterfaceService>();
        }

        private void OnReturnHandler()
        {
            ChangeStateAction?.Invoke(_previousState.stateType);
        }

        public override void OnStateChanged(BaseState previousState)
        {
            _previousState = previousState;
            _interfaceService.ShowWindow(_settingsWindowModel);
        }
    }
}