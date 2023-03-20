using System;
using DefaultNamespace;
using DefaultNamespace.States;
using SimpleInjector;
using UI;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private InterfaceView interfaceView;
    private Container _dependencyInjectionContainer;
    private StateMachine _stateMachine;

    private void Awake()
    {
        InitApplication();
    }

    private async void InitApplication()
    {
        try
        {
            Api.InitializeApi();

            RegisterServices();

            var gameDataController = _dependencyInjectionContainer.GetInstance<GameDataController>();
            await gameDataController.InitScoreMultiplier();

            var itemsHandler = _dependencyInjectionContainer.GetInstance<ItemsService>();
            itemsHandler.Init();

            _stateMachine = _dependencyInjectionContainer.GetInstance<StateMachine>();
            InitStates();

            interfaceView.Init(_dependencyInjectionContainer.GetInstance<InterfaceService>());

            _stateMachine.SetState(StateName.MenuState);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void InitStates()
    {
        _stateMachine.AddState(StateName.MenuState, new MenuState(_dependencyInjectionContainer));
        _stateMachine.AddState(StateName.GameSession, new GameSessionState(_dependencyInjectionContainer));
        _stateMachine.AddState(StateName.SettingsState, new SettingsState(_dependencyInjectionContainer));
    }

    private void RegisterServices()
    {
        _dependencyInjectionContainer = new Container();
        
        _dependencyInjectionContainer.Register<DataOperationService>(Lifestyle.Singleton);
        _dependencyInjectionContainer.Register<InterfaceService>(Lifestyle.Singleton);
        _dependencyInjectionContainer.Register<GameDataController>(Lifestyle.Singleton);
        _dependencyInjectionContainer.Register<ItemsService>(Lifestyle.Singleton);
        _dependencyInjectionContainer.Register<StateMachine>(Lifestyle.Singleton);

        _dependencyInjectionContainer.Verify();
    }
}