using Infrastructure.Constants;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Data.Ads
{
    public class BannerAdsProvider : BaseAdsProvider
    {
        public override string AdsId => Constants.AdsId.Banner;

        protected override string GetAndroidPlacementId() => AdsPlacementId.BannerAndroid;

        protected override string GetIOSPlacementId() => AdsPlacementId.BannerIOS;

        public override void Load()
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

            var loadOptions = new BannerLoadOptions
            {
                loadCallback = () =>
                {
                    Debug.LogError("[Ads] Banner load successful");
                },
                errorCallback = (msg) => Debug.LogError("[Ads] Banner load error: " + msg)
            };

            Advertisement.Banner.Load(GetPlacementId(), loadOptions);
        }
        
        public override void ShowAds()
        {
            var options = new BannerOptions
            {
                showCallback = () => OnAdsShowStartAction?.Invoke(GetPlacementId(), AdsId),
                hideCallback = () => {  }
            };

            Advertisement.Banner.Show(GetPlacementId(), options);
        }

        public void Hide(bool destroy = false) =>
            Advertisement.Banner.Hide(destroy);
    }
}