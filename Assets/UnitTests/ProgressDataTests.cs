using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Factories.Progress;
using Infrastructure.Services.Application;
using Infrastructure.Services.Log;
using Infrastructure.Services.Progress;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Zenject;

[TestFixture]
public class ProgressDataTests : ZenjectUnitTestFixture
{
    private const string UserId = "u1";
    
    private SaveLoadProgressService _saveLoadProgress;
    private IProgressFactory _progressFactory;
    private ApplicationLifecycleWatcher _applicationLifecycleWatcher;
    private IProgressRepository _progressRepository;
    private IExceptionLoggerService _editorExceptionLogger;

    public override void Setup()
    {
        base.Setup();
        _editorExceptionLogger = Substitute.For<IExceptionLoggerService>();
        _progressRepository = Substitute.For<IProgressRepository>();
        _progressFactory = Substitute.For<IProgressFactory>();
        
        Container.Bind<IExceptionLoggerService>().FromInstance(_editorExceptionLogger);
        Container.BindInterfacesAndSelfTo<ApplicationLifecycleWatcher>().AsSingle();
        Container.Bind<IProgressFactory>().FromInstance(_progressFactory);
        Container.Bind<IProgressRepository>().FromInstance(_progressRepository);
        Container.BindInterfacesAndSelfTo<SaveLoadProgressService>().AsSingle();
        
        _saveLoadProgress = Container.Resolve<SaveLoadProgressService>();
        _applicationLifecycleWatcher = Container.Resolve<ApplicationLifecycleWatcher>();
    }

    [Test]
    public void SaveProgress_WhenApplicationQuitting()
    {
        // Arrange
        var progressData = new ProgressData {UserId = UserId};
        _progressRepository.Load(UserId).Returns(Task.FromResult<ProgressData>(null));
        _progressFactory.CreateProgress(UserId).Returns(progressData);
        _progressRepository.Save(UserId, progressData).Returns(Task.CompletedTask);
        
        // Act
        _saveLoadProgress.OnReadyToLoadProgress(UserId);
        _applicationLifecycleWatcher.OnQuitAction?.Invoke();
        
        // Assert
        _progressRepository.Received(1).Save(UserId, progressData);
    }
    
    [Test]
    public void OnReadyToLoadProgress_WhenRepositoryLoadsNothing()
    {
        // Arrange
        _progressRepository.Load(UserId).Returns(Task.FromResult<ProgressData>(null));
        _progressFactory.CreateProgress(UserId).Returns(CreateFakeProgress());
        
        // Act
        _saveLoadProgress.OnReadyToLoadProgress(UserId);

        // Assert
        var readUserId = _saveLoadProgress.Read(progress => progress.UserId == UserId);
        Assert.IsTrue(readUserId);
    }

    [Test]
    public void OnReadyToLoadProgress_WhenRepositoryLoads()
    {
        // Arrange
        _progressRepository.Load(UserId).Returns(Task.FromResult(CreateFakeProgress()));
        _progressFactory.CreateProgress(UserId).ReturnsNull();
        
        // Act
        _saveLoadProgress.OnReadyToLoadProgress(UserId);

        // Assert
        var readUserId = _saveLoadProgress.Read(progress => progress.UserId == UserId);
        Assert.IsTrue(readUserId);
    }

    private ProgressData CreateFakeProgress() => new() {UserId = UserId};
}