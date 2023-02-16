using DefaultNamespace.States;

namespace DefaultNamespace
{
    public class GameHandler
    {
        private readonly StateMachine _stateMachine;
        private readonly InterfaceModel _interfaceModel;
        private readonly ItemsHandler _itemsHandler;
        private readonly GameDataController _gameDataController;
        
        public GameHandler(DataOperationService dataOperationService)
        {
            _stateMachine = new StateMachine();
            _interfaceModel = new InterfaceModel();
            _itemsHandler = new ItemsHandler(this);
            _gameDataController = new GameDataController(dataOperationService);
        }

        public async void Init()
        {
            InitStates();

            await _gameDataController.InitScoreMultiplier();
            
            _itemsHandler.Init();

            _stateMachine.SetState(StateName.MenuState);    
        }

        private void InitStates()
        {
            _stateMachine.AddState(StateName.MenuState, new MenuState(this));
            _stateMachine.AddState(StateName.GameSession, new GameSessionState(this));
            _stateMachine.AddState(StateName.SettingsState, new SettingsState(this));
        }

        public StateMachine stateMachine => _stateMachine;
        
        public InterfaceModel interfaceModel => _interfaceModel;

        public ItemsHandler itemsHandler => _itemsHandler;

        public GameDataController gameDataController => _gameDataController;
    }
}