using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Boosters;

namespace Infrastructure.Services
{
    public interface IBoostersService
    {
        List<BoosterModel> Boosters { get; }
        int BoostValue { get; }
        BoosterModel ActiveBooster { get; }
        void OnCompletePurchaseBooster(string boosterProductId);
    }
}