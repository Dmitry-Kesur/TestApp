using SimpleInjector;

namespace DefaultNamespace.States
{
    public class MenuState : BaseState
    {
        private readonly MenuWindowModel _menuWindowModel;
        private readonly InterfaceService _interfaceService;

        public MenuState(Container dependencyInjectionContainer)
        {
            _menuWindowModel = new MenuWindowModel(dependencyInjectionContainer);
            _interfaceService = dependencyInjectionContainer.GetInstance<InterfaceService>();
        }

        public override void OnStateEnter()
        {
            _interfaceService.ShowWindow(_menuWindowModel);
        }
    }
}