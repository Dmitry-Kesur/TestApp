using System;
using System.Collections.Generic;
using Infrastructure.Controllers.Levels;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services;
using Infrastructure.Strategy;
using Infrastructure.Views.GameEntities;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Zenject;

public class ItemsSpawnControllerTests
{
    private ItemsSpawnController _controller;
    
    private IItemsSpawnService _itemsSpawnService;
    private IItemsService _itemsService;
    private ICrashlyticsService _crashlyticsService;

    private ILevelModel _levelModel;

    [SetUp]
    public void SetUp()
    {
        var diContainer = new DiContainer();

        SetupBinds(diContainer);

        _controller = diContainer.Resolve<ItemsSpawnController>();

        ConfigureLevelModel();
    }

    private void SetupBinds(DiContainer diContainer)
    {
        _itemsSpawnService = Substitute.For<IItemsSpawnService>();
        _itemsService = Substitute.For<IItemsService>();
        _crashlyticsService = Substitute.For<ICrashlyticsService>();
        _levelModel = Substitute.For<ILevelModel>();
        
        diContainer.Bind<IItemsService>().FromInstance(_itemsService);
        diContainer.Bind<IItemsSpawnService>().FromInstance(_itemsSpawnService);
        diContainer.Bind<ICrashlyticsService>().FromInstance(_crashlyticsService);
        diContainer.Bind<ItemsSpawnController>().AsTransient();
    }

    private void ConfigureLevelModel()
    {
        _levelModel.DefaultItemsSpawnDelay.Returns(2.0f);
        _levelModel.DefaultDropItemsDuration.Returns(1.5f);
        _levelModel.MinimalDropItemsDuration.Returns(0.5f);
        _levelModel.MinimalItemsSpawnDelay.Returns(0.5f);
        _levelModel.CatchItemsToDecreaseSpawnDelay.Returns(5);
        _levelModel.SpawnDelayDecreaseValue.Returns(0.1f);
        _levelModel.DropItemsDecreaseDurationValue.Returns(0.1f);
    }

    [Test]
    public void OnStartLevelShouldSpawnItems()
    {
        // Act
        _controller.OnStartLevel();

        // Assert
        _itemsSpawnService.Received(1).UpdateSpawnDelay(Arg.Any<float>());
        _itemsSpawnService.Received(1).Spawn();
    }

    [Test]
    public void OnPauseLevelShouldDisableSpawn()
    {
        // Arrange
        var itemView = new GameObject().AddComponent<ItemView>();
        var itemList = new List<ItemView> { itemView };
        InjectItemList(itemList);

        // Act
        _controller.OnPauseLevel();

        // Assert
        _itemsSpawnService.Received(1).DisableSpawn();
        Assert.IsTrue(itemView.Paused);
    }

    [Test]
    public void OnResumeLevelShouldEnableSpawn()
    {
        // Arrange
        var itemView = new GameObject().AddComponent<ItemView>();
        var itemList = new List<ItemView> { itemView };
        InjectItemList(itemList);

        // Act
        _controller.OnResumeLevel();

        // Assert
        _itemsSpawnService.Received(1).EnableSpawn();
        Assert.False(itemView.Paused);
    }

    [Test]
    public void UpdateItemsByTotalCatchAmount()
    {
        // Arrange
        _controller.SetModel(_levelModel);
        var totalCatchItems = 5;

        // Act
        _controller.UpdateItemsByTotalCatchAmount(totalCatchItems);

        // Assert
        _itemsSpawnService.Received(1).UpdateSpawnDelay(Arg.Any<float>());
    }

    private void InjectItemList(List<ItemView> items)
    {
        var field = typeof(ItemsSpawnController).GetField("_items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(_controller, items);
    }
}