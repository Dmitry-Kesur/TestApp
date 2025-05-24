using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Preloader;
using Infrastructure.Data.Rewards;
using Infrastructure.Models.GameEntities.Rewards;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Log;
using Infrastructure.Services.Preloader;

namespace Infrastructure.Services.Reward
{
    public class RewardsService : IRewardsService, ILoadableService
    {
        private readonly LocalAddressableService _localAddressableService;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        private List<RewardStaticData> _rewardsData;
        
        public RewardsService(LocalAddressableService localAddressableService, IExceptionLoggerService exceptionLoggerService)
        {
            _localAddressableService = localAddressableService;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public async Task Load()
        {
            _rewardsData = await _localAddressableService.LoadScriptableCollectionFromGroupAsync<RewardStaticData>(AddressableGroupNames.RewardsGroup);
        }

        public LoadingStage LoadingStage => LoadingStage.LoadingRewards;

        public List<RewardModel> GetRewardsModelsByData(List<RewardReceiveData> receiveRewardsData)
        {
            if (receiveRewardsData == null)
            {
                _exceptionLoggerService.Log("[rewards-data] rewards data is null");
                return null;
            }
            
            List<RewardModel> rewardModels = new();
            
            var staticById = _rewardsData.ToDictionary(d => d.Id);
            var receiveById = receiveRewardsData.ToDictionary(d => d.Id);

            foreach (var rewardId in receiveById.Keys)
            {
                if (staticById.TryGetValue(rewardId, out var staticData))
                {
                    var amount = receiveById[rewardId].Amount;
                    rewardModels.Add(new RewardModel(staticData, amount));
                }
                else
                {
                    _exceptionLoggerService.Log($"Missing static reward data for id {rewardId}");
                }
            }


            return rewardModels;
        }
    }
}