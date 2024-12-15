using Infrastructure.Data.Rewards;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Rewards;
using Infrastructure.Services;

namespace Infrastructure.Controllers.Rewards
{
    public class CurrencyReceiveRewardsController : BaseReceiveRewardsController
    {
        private readonly ICurrencyService _currencyService;
        
        public CurrencyReceiveRewardsController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        
        public override void ReceiveReward(RewardModel rewardModel)
        {
            _currencyService.IncreaseCurrency(rewardModel.Amount);
        }

        public override RewardType RewardType => RewardType.Currency;
    }
}