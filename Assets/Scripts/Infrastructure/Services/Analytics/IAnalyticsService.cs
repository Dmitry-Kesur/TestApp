namespace Infrastructure.Services.Analytics
{
    public interface IAnalyticsService : IFirebaseInitialize
    {
        void LogWinLevel(int level);
        void LogLoseLevel(int level);
        void LogPurchaseProduct(int productId);
        void LogInAppPurchaseProduct(string productId);
        void LogInAppPurchaseProductRestore(string productId);
    }
}