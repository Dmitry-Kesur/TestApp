namespace Infrastructure.Factories.Progress
{
    public interface IProgressFactory
    {
        Data.PlayerProgress.Progress CreateNewProgress(string userId);
    }
}