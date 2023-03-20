using SimpleInjector;

namespace DefaultNamespace.States
{
    public class SettingsState : BaseState
    {
        private readonly SettingsWindowModel _settingsWindowModel;
        private readonly InterfaceService _interfaceService;

        public SettingsState(Container dependencyInjectionContainer)
        {
            _settingsWindowModel = new SettingsWindowModel(dependencyInjectionContainer);
            _interfaceService = dependencyInjectionContainer.GetInstance<InterfaceService>();
        }

        public override void OnStateEnter()
        {
            _interfaceService.ShowWindow(_settingsWindowModel);
        }
    }
}