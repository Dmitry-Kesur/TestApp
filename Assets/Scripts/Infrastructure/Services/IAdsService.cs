using System;

namespace Infrastructure.Services
{
    public interface IAdsService
    {
        void ShowAds(string adsId);
        Action OnShowCompleteAdsAction { get; set; }
    }
}