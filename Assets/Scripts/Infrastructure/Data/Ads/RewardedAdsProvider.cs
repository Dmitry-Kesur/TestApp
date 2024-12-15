using UnityEngine.Advertisements;

namespace Infrastructure.Data.Ads
{
    public class RewardedAdsProvider : BaseAdsProvider
    {
        public override string AdsId =>
            Constants.AdsId.Rewarded;

        protected override string GetAndroidPlacementId() =>
            "Rewarded_Android";

        protected override string GetIOSPlacementId() =>
            "Rewarded_iOS";

        public override void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                OnAdsShowCompleteAction?.Invoke();
            }
        }
    }
}