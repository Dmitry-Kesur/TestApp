using Infrastructure.Enums;
using Infrastructure.Services.Bootstrap;

namespace Infrastructure.Services.Analytics
{
    public interface IAnalyticsService : IFirebaseInitialize
    {
        void LogWinLevel(int level);
        void LogLoseLevel(int level);
        void LogPurchaseProduct(int productId);
        void LogCompleteInAppPurchaseProduct(string productId);
        void LogFailedInAppPurchaseProduct(string productId);
        void LogInAppPurchaseProductRestore(string productId);
        void LogReceiveReward(RewardType rewardType);
    }
}