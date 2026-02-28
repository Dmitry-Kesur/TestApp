using Infrastructure.Data.Boosters;
using Infrastructure.Models.GameEntities.Boosters;
using Infrastructure.Models.GameEntities.Resources;

namespace Infrastructure.Factories.Booster
{
    public class BoosterFactory
    {
        public BoosterModel Create(BoosterData boosterData, ResourceModel boosterResource)
        {
            var boosterModel = new BoosterModel(boosterData, boosterResource);
            return boosterModel;
        }
    }
}