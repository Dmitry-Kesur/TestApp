using SimpleInjector;

namespace DefaultNamespace.States
{
    public class MenuState : BaseState
    {
        private readonly MenuWindowModel _menuWindowModel;
        private readonly InterfaceService _interfaceService;

        public MenuState(StateType stateType, Container container) : base(stateType, container)
        {
            _menuWindowModel = new MenuWindowModel(container)
            {
                OnPlayButtonClickAction = OnPlayButtonClickHandler,
                OnSettingsButtonClickAction = OnSettingsButtonClickHandler
            };

            _interfaceService = container.GetInstance<InterfaceService>();
        }

        private void OnSettingsButtonClickHandler()
        {
            ChangeStateAction?.Invoke(StateType.SettingsState);
        }

        private void OnPlayButtonClickHandler()
        {
            ChangeStateAction?.Invoke(StateType.GameSession);
        }

        public override void OnStateChanged(BaseState previousState)
        {
            _interfaceService.ShowWindow(_menuWindowModel);
        }
    }
}