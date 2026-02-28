using Infrastructure.Constants;
using UnityEngine.Advertisements;

namespace Infrastructure.Data.Ads
{
    public class RewardedAdsProvider : BaseAdsProvider
    {
        public override string AdsId =>
            Constants.AdsId.Rewarded;

        protected override string GetAndroidPlacementId() =>
            AdsPlacementId.RewardedAndroid;

        protected override string GetIOSPlacementId() =>
            AdsPlacementId.RewardedIOS;
    }
}