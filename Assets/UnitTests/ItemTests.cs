using System.Collections.Generic;
using Infrastructure.Controllers.Levels;
using Infrastructure.Data.Items;
using Infrastructure.Factories.Level;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services.Items;
using Infrastructure.Services.Log;
using Infrastructure.Views.GameEntities;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Zenject;

[TestFixture]
public class ItemTests : ZenjectUnitTestFixture
{
    private ItemsSpawnService _itemsSpawnService;
    private IItemsService _itemsService;
    private IExceptionLoggerService _exceptionLoggerService;
    private ILevelModel _levelModel;
    private ItemsSpawnController _itemsSpawnController;
    private ItemsStrategyFactory _itemsStrategyFactory;
    private IItemViewsFactory _itemViewsFactory;

    private ItemView _itemView;

    public override void Setup()
    {
        base.Setup();
        _itemsService = Substitute.For<IItemsService>();
        _exceptionLoggerService = Substitute.For<IExceptionLoggerService>();
        _levelModel = Substitute.For<ILevelModel>();
        _itemViewsFactory = Substitute.For<IItemViewsFactory>();

        Container.BindInterfacesAndSelfTo<ItemsSpawnService>().AsSingle();
        Container.Bind<IItemsService>().FromInstance(_itemsService);
        Container.Bind<ILevelModel>().FromInstance(_levelModel);
        Container.Bind<ItemsSpawnController>().AsSingle();
        Container.Bind<IExceptionLoggerService>().To<EditorExceptionLoggerService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ItemsStrategyFactory>().AsSingle();
        Container.Bind<IItemViewsFactory>().FromInstance(_itemViewsFactory);

        _itemsSpawnController = Container.Resolve<ItemsSpawnController>();
        _itemsStrategyFactory = Container.Resolve<ItemsStrategyFactory>();
        _itemsSpawnService = Container.Resolve<ItemsSpawnService>();
    }

    [Test]
    public void SetModel_ShouldInitializeSpawnDelays()
    {
        _levelModel.DefaultItemsSpawnDelay.Returns(2.0f);
        _levelModel.DefaultDropItemsDuration.Returns(5.0f);
        _itemsSpawnController.SetModel(_levelModel);
        Assert.AreEqual(5.0f, _itemsSpawnController.GetDropItemsDuration());
    }

    [Test]
    public void DecreaseDropDuration_WhenCatchThresholdReached()
    {
        // Arrange
        _levelModel.MinimalDropItemsDuration.Returns(2.2f);
        _levelModel.DefaultDropItemsDuration.Returns(5.5f);
        _levelModel.DropItemsDecreaseDurationValue.Returns(0.3f);
        _levelModel.CatchItemsToDecreaseSpawnDelay.Returns(7);

        _itemsSpawnController.SetModel(_levelModel);

        // Act
        _itemsSpawnController.UpdateItemsByTotalCatchAmount(7);

        // Assert
        Assert.Less(_itemsSpawnController.GetDropItemsDuration(), _levelModel.DefaultDropItemsDuration);
    }

    [Test]
    public void OnStartLevel_ShouldCallSpawnItems_WhenCalled()
    {
        // Arrange
        var itemData = ScriptableObject.CreateInstance<ItemData>();
        itemData.SpawnChance = 0.5f;
        var itemModel = new ItemModel(itemData);

        _itemView = new GameObject().AddComponent<ItemView>();

        _itemViewsFactory.GetItem().Returns(_itemView);

        bool spawnCalled = false;
        _itemsSpawnService.OnSpawnItemAction = _ => { spawnCalled = true; };
        
        _itemsSpawnService.SetItemModels(new List<ItemModel> { itemModel });

        // Act
        _itemsSpawnController.OnStartLevel();

        // Assert
        Assert.IsTrue(spawnCalled);
    }

    [Test]
    public void RemoveItem_ShouldCallReleaseItem()
    {
        // Arrange
        _itemView = new GameObject().AddComponent<ItemView>();

        // Act
        _itemsSpawnService.RemoveItem(_itemView);

        // Assert
        _itemViewsFactory.Received(1).ReleaseItem(_itemView);
    }

    [Test]
    public void Clear_CallsClearOnItemViewsFactory()
    {
        // Act
        _itemsSpawnService.Clear();
        
        // Assert
        _itemViewsFactory.Received(1).Clear();
    }

    [Test]
    public void OnPause_AllSpawnedItemsArePaused()
    {
        // Arrange
        _itemView = new GameObject().AddComponent<ItemView>();
        
        // Act
        _itemsSpawnService.OnSpawnItemAction?.Invoke(_itemView);
        _itemsSpawnController.OnPause();
        
        // Assert
        Assert.IsTrue(_itemView.Paused);
    }
    
    [Test]
    public void OnResume_AllSpawnedItemsAreResumed()
    {
        // Arrange
        _itemView = new GameObject().AddComponent<ItemView>();
        
        // Act
        _itemsSpawnService.OnSpawnItemAction?.Invoke(_itemView);
        _itemsSpawnController.OnPause();
        
        _itemsSpawnController.OnResume();
        
        // Assert
        Assert.IsFalse(_itemView.Paused);
    }
    
    [TearDown]
    public void TearDown()
    {
        // Cleanup
        if (_itemView == null)
            return;
        
        GameObject.DestroyImmediate(_itemView.gameObject);
        _itemView = null;
    }
}