using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Factories.Purchase
{
    public interface IProgressFactory
    {
        PlayerProgress CreateNewProgress(string userId);
    }
}