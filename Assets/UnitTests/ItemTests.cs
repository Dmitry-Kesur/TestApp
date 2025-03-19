using System;
using Infrastructure.Controllers.Levels;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Services;
using Infrastructure.Services.Items;
using Infrastructure.Services.Log;
using NSubstitute;
using NUnit.Framework;
using Zenject;

[TestFixture]
public class ItemTests : ZenjectUnitTestFixture
{
    private IItemsSpawnService _itemsSpawnService;
    private IItemsService _itemsService;
    private IExceptionLoggerService _exceptionLoggerService;
    private ILevelModel _levelModel;
    private ItemsSpawnController _itemsSpawnController;
    
    [SetUp]
    public void SetUp()
    {
        _itemsSpawnService = Substitute.For<IItemsSpawnService>();
        _itemsService = Substitute.For<IItemsService>();
        _exceptionLoggerService = Substitute.For<IExceptionLoggerService>();
        _levelModel = Substitute.For<ILevelModel>();

        Container.Bind<IItemsSpawnService>().FromInstance(_itemsSpawnService);
        Container.Bind<IItemsService>().FromInstance(_itemsService);
        Container.Bind<IExceptionLoggerService>().FromInstance(_exceptionLoggerService);
        Container.Bind<ILevelModel>().FromInstance(_levelModel);
        Container.Bind<ItemsSpawnController>().AsSingle();

        _itemsSpawnController = Container.Resolve<ItemsSpawnController>();
    }
    
    [Test]
    public void OnStartLevel_ShouldCallSpawnItems()
    {
        _itemsSpawnController.SetModel(_levelModel);
        _itemsSpawnController.OnStartLevel();
        _itemsSpawnService.Received().Spawn();
    }
    
    [Test]
    public void OnStartLevel_ShouldInvokeSpawnItems_WhenCalled()
    {
        // Act
        _itemsSpawnController.OnStartLevel();

        // Assert
        _itemsSpawnService.Received(1).Spawn();
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
    public void SetModel_WithNullModel_ShouldThrowException()
    {
        var ex = Assert.Throws<NullReferenceException>(() => _itemsSpawnController.SetModel(null));
        Assert.AreEqual("Level model is null", ex.Message);
        _exceptionLoggerService.Received(1).LogError("Level model is null");
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
    public void Clear_ShouldResetValues_AndDisableSpawn()
    {
        // Arrange
        _itemsSpawnController.SetModel(_levelModel);
        
        // Act
        _itemsSpawnController.Clear();
        
        // Assert
        _itemsSpawnService.Received(1).Clear();
    }
}