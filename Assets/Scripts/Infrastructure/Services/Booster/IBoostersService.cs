using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Boosters;

namespace Infrastructure.Services.Booster
{
    public interface IBoostersService
    {
        int BoostValue { get; }
        bool HasBoosterToActivate { get; }
        BoosterModel ActiveBooster { get; }
        IReadOnlyList<BoosterModel> GetAvailableBoosters();
        Action OnBoosterActivatedAction { get; set; }
        Action OnBoosterDeactivatedAction { get; set; }
    }
}