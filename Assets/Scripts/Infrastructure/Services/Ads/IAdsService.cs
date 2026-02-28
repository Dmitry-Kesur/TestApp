using System;

namespace Infrastructure.Services.Ads
{
    public interface IAdsService
    {
        void ShowAds(string adsId);
        void HideBanner();
        Action<string> OnAdsShowCompletedAction { get; set; }
    }
}