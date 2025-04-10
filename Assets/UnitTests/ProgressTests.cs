using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Factories.Purchase;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using NSubstitute;
using NUnit.Framework;
using Zenject;

[TestFixture]
public class ProgressTests : ZenjectUnitTestFixture
{
    private IProgressService _progressService;
    private ISaveLoadProgressService _saveLoadProgress;
    private List<IProgressUpdater> _progressUpdaters;
    private IProgressFactory _progressFactory;

    public override void Setup()
    {
        base.Setup();
        _saveLoadProgress = Substitute.For<ISaveLoadProgressService>();
        _progressUpdaters = new List<IProgressUpdater> { Substitute.For<IProgressUpdater>() };
        _progressFactory = Substitute.For<IProgressFactory>();

        Container.Bind<IProgressService>().To<ProgressService>().AsSingle();
        Container.Bind<ISaveLoadProgressService>().FromInstance(_saveLoadProgress);
        Container.Bind<List<IProgressUpdater>>().FromInstance(_progressUpdaters);
        Container.Bind<IProgressFactory>().FromInstance(_progressFactory);
        Container.Bind<Progress>().AsTransient();

        _progressService = Container.Resolve<IProgressService>();
    }

    [Test]
    public void LoadPlayerProgress_ShouldLoadExistingProgress()
    {
        // Arrange
        string userId = "testUser";
        var mockProgress = new Progress();
        _saveLoadProgress.LoadProgress(userId).Returns(Task.FromResult(mockProgress));

        // Act
        _progressService.LoadPlayerProgress(userId);

        // Assert
        _progressUpdaters[0].Received(1).OnLoadProgress(mockProgress);
    }
    
    [Test]
    public void LoadPlayerProgress_ShouldCreateNewProgressIfNoneExists()
    {
        // Arrange
        string userId = "testUser";
        var newProgress = new Progress
        {
            UserId = userId
        };

        _saveLoadProgress.LoadProgress(userId).Returns(Task.FromResult<Progress>(null)); 
        _progressFactory.CreateNewProgress(userId).Returns(newProgress);

        // Act
        _progressService.LoadPlayerProgress(userId);

        // Assert
        _progressFactory.Received(1).CreateNewProgress(userId);
    }

}