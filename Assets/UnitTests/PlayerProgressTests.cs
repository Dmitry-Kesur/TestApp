using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Factories;
using Infrastructure.Services;
using Infrastructure.Services.PlayerProgressUpdaters;
using NSubstitute;
using NUnit.Framework;

public class PlayerProgressTests
{
    private IProgressService _progressService;
    private ISaveLoadProgressService _saveLoadProgress;
    private List<IProgressUpdater> _progressUpdaters;
    private IProgressFactory _progressFactory;

    [SetUp]
    public void Setup()
    {
        _saveLoadProgress = Substitute.For<ISaveLoadProgressService>();
        _progressUpdaters = new List<IProgressUpdater> { Substitute.For<IProgressUpdater>() };
        _progressFactory = Substitute.For<IProgressFactory>();
        _progressService = new ProgressService(_saveLoadProgress, _progressUpdaters, _progressFactory);
    }

    [Test]
    public void LoadPlayerProgress_ShouldLoadExistingProgress()
    {
        // Arrange
        string userId = "testUser";
        var mockProgress = new PlayerProgress();
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
        var newProgress = new PlayerProgress
        {
            UserId = userId
        };

        _saveLoadProgress.LoadProgress(userId).Returns(Task.FromResult<PlayerProgress>(null)); 
        _progressFactory.CreateNewProgress(userId).Returns(newProgress);

        // Act
        _progressService.LoadPlayerProgress(userId);

        // Assert
        _progressFactory.Received(1).CreateNewProgress(userId);
    }

}