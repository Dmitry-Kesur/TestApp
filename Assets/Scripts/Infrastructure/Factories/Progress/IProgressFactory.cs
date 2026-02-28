using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Factories.Progress
{
    public interface IProgressFactory
    {
        ProgressData CreateProgress(string userId);
    }
}