namespace Infrastructure.Services
{
    public interface IAnalyticsService : IInitializeAsync
    {
        void LogWinLevel(int level);
        void LogLoseLevel(int level);
        void LogPurchaseProduct(int productId);
    }
}