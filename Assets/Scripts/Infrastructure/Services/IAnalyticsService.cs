namespace Infrastructure.Services
{
    public interface IAnalyticsService : IFirebaseInitialize
    {
        void LogWinLevel(int level);
        void LogLoseLevel(int level);
        void LogPurchaseProduct(int productId);
    }
}