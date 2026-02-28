using Infrastructure.Enums;

namespace Infrastructure.Services.Analytics
{
    public interface IAnalyticsService
    {
        void LogWinLevel(int level);
        void LogLoseLevel(int level);
        void LogPurchaseInGameProduct(int productId);
        void LogCompleteInAppPurchaseProduct(string productId);
        void LogFailedInAppPurchaseProduct(string productId);
        void LogInAppPurchaseProductRestore(string productId);
        void LogReceiveReward(int rewardId);
        void LogAdsImpression(string placementId, string adsId);
    }
}