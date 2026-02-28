using System.Collections.Generic;
using Infrastructure.Controllers.Levels;
using Infrastructure.Data.Items;
using Infrastructure.Factories.Level;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services.Items;
using Infrastructure.Services.Log;
using Infrastructure.Strategy;
using Infrastructure.Views.GameEntities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UnityEngine;
using Zenject;

[TestFixture]
public class ItemTests : ZenjectUnitTestFixture
{
    private ItemsSpawnService _itemsSpawnService;
    private ILevelSession _levelSession;
    private ItemsSpawnController _itemsSpawnController;
    private IItemViewsFactory _itemViewsFactory;
    private IItemsStrategyFactory _itemsStrategyFactory;
    private IItemsService _itemsService;

    private ItemView _itemView;

    public override void Setup()
    {
        base.Setup();
        _levelSession = Substitute.For<ILevelSession>();
        _itemViewsFactory = Substitute.For<IItemViewsFactory>();
        _itemsService = Substitute.For<IItemsService>();

        Container.BindInterfacesAndSelfTo<ItemsSpawnService>().AsSingle();
        Container.Bind<ILevelSession>().FromInstance(_levelSession);
        Container.Bind<ItemsSpawnController>().AsSingle();
        Container.Bind<IExceptionLoggerService>().To<EditorExceptionLoggerService>().AsSingle();
        Container.Bind<IItemViewsFactory>().FromInstance(_itemViewsFactory);
        Container.Bind<IItemsStrategyFactory>().To<ItemsStrategyFactory>().AsSingle();
        Container.Bind<IItemsService>().FromInstance(_itemsService).AsSingle();

        _itemsSpawnController = Container.Resolve<ItemsSpawnController>();
        _itemsSpawnService = Container.Resolve<ItemsSpawnService>();
        _itemsStrategyFactory = Container.Resolve<IItemsStrategyFactory>();
        _itemsService = Container.Resolve<IItemsService>();
    }

    [Test]
    public void SetModel_WhenModelHasValidDelays_ShouldInitializeSpawnDelays()
    {
        const float expectedSpawnDelay = 2.0f;
        const float expectedDropDuration = 5.0f;

        _levelSession.DefaultItemsSpawnDelay.Returns(expectedSpawnDelay);
        _levelSession.DefaultDropItemsDuration.Returns(expectedDropDuration);

        _itemsSpawnController.SetModel(_levelSession);

        Assert.AreEqual(expectedSpawnDelay, _itemsSpawnController.GetItemsSpawnDelay());
        Assert.AreEqual(expectedDropDuration, _itemsSpawnController.GetDropItemsDuration());
    }

    [Test]
    public void DecreaseDropDuration_WhenCatchAmountReachesThreshold()
    {
        // Arrange
        const float defaultDuration = 5.5f;
        const float minimalDuration = 2.2f;
        const float decreaseValue = 0.3f;
        const int decreaseThreshold = 7;

        _levelSession.MinimalDropItemsDuration.Returns(minimalDuration);
        _levelSession.DefaultDropItemsDuration.Returns(defaultDuration);
        _levelSession.DropItemsDecreaseDurationValue.Returns(decreaseValue);
        _levelSession.CatchItemsToDecreaseSpawnDelay.Returns(decreaseThreshold);

        _itemsSpawnController.SetModel(_levelSession);

        // Act
        _itemsSpawnController.UpdateItemsByTotalCatchAmount(decreaseThreshold);

        // Assert
        var actualDuration = _itemsSpawnController.GetDropItemsDuration();
        Assert.Less(actualDuration, defaultDuration);
    }

    [Test]
    public void OnStartLevel_ShouldCallSpawnItems_WhenCalled()
    {
        // Arrange
        var itemData = ScriptableObject.CreateInstance<ItemData>();
        itemData.SpawnChance = 0.5f;
        var itemModel = new ItemModel(itemData);

        _itemView = CreateItemView();

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
        _itemView = CreateItemView();
        var itemData = ScriptableObject.CreateInstance<ItemData>();
        var itemModel = new ItemModel(itemData);
        var itemModels = new List<ItemModel> { itemModel };
        _itemsSpawnController.SetItems(itemModels);
        
        // Act
        itemModel.OnRemoveItem(_itemView);

        // Assert
        _itemViewsFactory.Received(1).ReleaseItem(_itemView);
    }

    [Test]
    public void OnPause_AllSpawnedItemsArePaused()
    {
        // Arrange
        _itemView = CreateItemView();

        // Act
        _itemsSpawnService.OnSpawnItemAction?.Invoke(_itemView);
        _itemsSpawnController.Pause();

        // Assert
        Assert.IsTrue(_itemView.Paused);
        Assert.IsFalse(_itemsSpawnService.Enabled);
    }

    [Test]
    public void OnResume_AllSpawnedItemsAreResumed()
    {
        // Arrange
        _itemView = CreateItemView();

        // Act
        _itemsSpawnService.OnSpawnItemAction?.Invoke(_itemView);
        _itemsSpawnController.Pause();

        _itemsSpawnController.OnResume();

        // Assert
        Assert.IsFalse(_itemView.Paused);
        Assert.IsTrue(_itemsSpawnService.Enabled);
    }

    [Test]
    public void ShouldReturnSingleItemStrategy_WhenMultipleItemIdsProvided()
    {
        // Arrange
        var itemData = ScriptableObject.CreateInstance<ItemData>();
        var itemModel = new ItemModel(itemData);
        _itemsService.GetSelectedItem().Returns(itemModel);

        var itemIds = new List<int> { 1, 5, 10 };
        
        // Act
        var itemsStrategy = _itemsStrategyFactory.GetItemsStrategy(itemIds);

        // Assert
        Assert.That(itemsStrategy, Is.InstanceOf<SingleItemStrategy>());
    }

    [Test]
    public void ShouldReturnMultipleItemsStrategy_WhenNoSelectedItem()
    {
        // Arrange
        _itemsService.GetSelectedItem().ReturnsNull();
    
        var itemIds = new List<int> { 1, 5, 10 };

        // Act
        var itemsStrategy = _itemsStrategyFactory.GetItemsStrategy(itemIds);

        // Assert
        Assert.That(itemsStrategy, Is.InstanceOf<MultipleItemsStrategy>());
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

    private ItemView CreateItemView() =>
        new GameObject().AddComponent<ItemView>();
}