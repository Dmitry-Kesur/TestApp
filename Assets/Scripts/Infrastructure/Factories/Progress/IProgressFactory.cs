using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Factories.Purchase
{
    public interface IProgressFactory
    {
        Data.PlayerProgress.Progress CreateNewProgress(string userId);
    }
}