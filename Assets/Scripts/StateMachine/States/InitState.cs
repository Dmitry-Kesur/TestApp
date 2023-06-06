using System.Threading.Tasks;
using DefaultNamespace;
using SimpleInjector;
using UI;
using UnityEngine;

public class InitState : BaseState
{
    private InterfaceView _interfaceView;

    public InitState(StateType stateType, Container container) : base(stateType, container)
    {
       
    }

    public override async void OnStateChanged(BaseState previousState)
    {
        await Init();
        
        ChangeStateAction?.Invoke(StateType.MenuState);
    }

    private async Task Init()
    {
        InitInterfaceView();
        
        var gameDataService = container.GetInstance<GameDataService>();
        await gameDataService.Init();
    }

    private void InitInterfaceView()
    {
        var prefab = Resources.Load<InterfaceView>("Prefabs/UI/Views/InterfaceView");
        _interfaceView = Object.Instantiate(prefab);
        
        var interfaceService = container.GetInstance<InterfaceService>();
        _interfaceView.Init(interfaceService);
    }
}