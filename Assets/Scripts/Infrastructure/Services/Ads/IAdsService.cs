using System;

namespace Infrastructure.Services.Ads
{
    public interface IAdsService
    {
        void ShowAds(string adsId);
        Action OnShowCompleteAdsAction { get; set; }
    }
}