using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Factories.Progress;
using Infrastructure.Factories.Purchase;
using Infrastructure.Services;
using Infrastructure.Services.Application;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using NSubstitute;
using NUnit.Framework;
using Zenject;

[TestFixture]
public class ProgressTests : ZenjectUnitTestFixture
{
    private const string UserId = "testUser";
    
    private IProgressService _progressService;
    private ISaveLoadProgressService _saveLoadProgress;
    private List<IProgressUpdater> _progressUpdaters;
    private IProgressFactory _progressFactory;
    private ApplicationFocusWatcher _applicationFocusWatcher;

    public override void Setup()
    {
        base.Setup();
        _saveLoadProgress = Substitute.For<ISaveLoadProgressService>();
        _progressUpdaters = new List<IProgressUpdater> { Substitute.For<IProgressUpdater>() };
        _progressFactory = Substitute.For<IProgressFactory>();
        _applicationFocusWatcher = Substitute.For<ApplicationFocusWatcher>();
        
        Container.Bind<ApplicationFocusWatcher>().FromInstance(_applicationFocusWatcher);
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
        var progress = new Progress();
        _saveLoadProgress.LoadProgress(UserId).Returns(Task.FromResult(progress));

        // Act
        _progressService.LoadPlayerProgress(UserId);

        // Assert
        _progressUpdaters[0].Received(1).OnLoadProgress(progress);
    }
    
    [Test]
    public void LoadPlayerProgress_ShouldCreateNewProgressIfNoneExists()
    {
        // Arrange
        var progress = new Progress
        {
            UserId = UserId
        };

        _saveLoadProgress.LoadProgress(UserId).Returns(Task.FromResult<Progress>(null)); 
        _progressFactory.CreateNewProgress(UserId).Returns(progress);

        // Act
        _progressService.LoadPlayerProgress(UserId);

        // Assert
        _progressFactory.Received(1).CreateNewProgress(UserId);
    }

    [Test]
    public void SaveProgress_WhenFocusLost_ShouldSavePlayerProgress()
    {
        // Arrange
        var progress = new Progress();
        
        _saveLoadProgress.LoadProgress(UserId).Returns(Task.FromResult(progress).Result);
        
        _applicationFocusWatcher.OnFocusOut = _progressService.SavePlayerProgress;
        
        _progressService.LoadPlayerProgress(UserId);
        
        // Act
        _applicationFocusWatcher.OnFocusOut?.Invoke();

        // Assert
        _saveLoadProgress.Received(1).SaveProgress(Arg.Any<Progress>());
    }
}