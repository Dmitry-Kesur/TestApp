using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Factories
{
    public interface IProgressFactory
    {
        PlayerProgress CreateNewProgress(string userId);
    }
}