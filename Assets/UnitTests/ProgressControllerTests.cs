using NSubstitute;
using NUnit.Framework;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Services;
using Infrastructure.Controllers.Levels;
using Zenject;

[TestFixture]
public class ProgressControllerTests
{
    private ProgressController _progressController;
    private IHudService _hudService;
    private ILevelModel _levelModel;

    [SetUp]
    public void SetUp()
    {
        var diContainer = new DiContainer();
        _hudService = Substitute.For<IHudService>();
        _levelModel = Substitute.For<ILevelModel>();
        
        _levelModel.FailItemsMaximum.Returns(3);
        _levelModel.ScorePointsToWin.Returns(10);

        diContainer.Bind<ILevelModel>().FromInstance(_levelModel);
        diContainer.Bind<IHudService>().FromInstance(_hudService);
        diContainer.Bind<ProgressController>().AsTransient();
        _progressController = diContainer.Resolve<ProgressController>();

        _progressController.SetModel(_levelModel);
    }

    [Test]
    public void WhenUpdateProgressByFailItem()
    {
        // Act
        _progressController.UpdateProgressByFailItem();

        // Assert
        Assert.AreEqual(1, _progressController.TotalFailItems);
        _hudService.Received(1).UpdateHud();
    }

    [Test]
    public void WhenReachedMaximumFailItems()
    {
        // Arrange
        bool maxFailsReached = false;
        _progressController.OnReachedMaximumFailItems += () => maxFailsReached = true;

        // Act
        _progressController.UpdateProgressByFailItem();
        _progressController.UpdateProgressByFailItem();
        _progressController.UpdateProgressByFailItem();

        // Assert
        Assert.IsTrue(maxFailsReached);
    }

    [Test]
    public void WhenIncreaseAndUpdateTotalScore()
    {
        // Act
        _progressController.UpdateProgressByCatchItem(5);

        // Assert
        Assert.AreEqual(1, _progressController.TotalCatchItems);
        Assert.AreEqual(5, _progressController.TotalLevelScore);
        _hudService.Received(1).UpdateHud();
    }

    [Test]
    public void WhenReachedScorePointsToWin()
    {
        // Arrange
        bool scoreToWinReached = false;
        _progressController.OnReachScoreToWin += () => scoreToWinReached = true;

        // Act
        _progressController.UpdateProgressByCatchItem(5);
        _progressController.UpdateProgressByCatchItem(5);

        // Assert
        Assert.IsTrue(scoreToWinReached);
    }

    [Test]
    public void WhenClearAllCounters()
    {
        // Arrange
        _progressController.UpdateProgressByFailItem();
        _progressController.UpdateProgressByCatchItem(5);

        // Act
        _progressController.Clear();

        // Assert
        Assert.AreEqual(0, _progressController.TotalFailItems);
        Assert.AreEqual(0, _progressController.TotalCatchItems);
        Assert.AreEqual(0, _progressController.TotalLevelScore);
    }
}
