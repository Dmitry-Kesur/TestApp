using System;
using DefaultNamespace;
using SimpleInjector;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private Container _container;

    private void Awake()
    {
        InitApplication();
    }

    private void InitApplication()
    {
        try
        {
            ApiService.Init();
            RegisterServices();

            var stateMachine = _container.GetInstance<StateMachine>();
            stateMachine.Init(_container);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void RegisterServices()
    {
        _container = new Container();
        
        _container.Register<InterfaceService>(Lifestyle.Singleton);
        _container.Register<GameDataService>(Lifestyle.Singleton);
        _container.Register<ItemsService>(Lifestyle.Singleton);
        _container.Register<StateMachine>(Lifestyle.Singleton);

        _container.Verify();
    }
}