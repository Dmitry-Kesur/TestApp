using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Preloader;
using Infrastructure.Data.Rewards;
using Infrastructure.Models.GameEntities.Rewards;

namespace Infrastructure.Services
{
    public class RewardsService : IRewardsService, ILoadableService
    {
        private readonly LocalAddressableService _localAddressableService;

        private List<RewardStaticData> _rewardsData;
        
        public RewardsService(LocalAddressableService localAddressableService)
        {
            _localAddressableService = localAddressableService;
        }

        public async Task Load()
        {
            _rewardsData = await _localAddressableService.LoadScriptableCollectionFromGroupAsync<RewardStaticData>(AddressableGroupNames.RewardsGroup);
        }

        public LoadingStage LoadingStage => LoadingStage.LoadingRewards;

        public List<RewardModel> GetRewardsModelsByData(List<RewardReceiveData> receiveRewardsData)
        {
            List<RewardModel> rewardModels = new();
            
            var rewardIds = receiveRewardsData.ConvertAll(data => data.Id);
            foreach (var rewardId in rewardIds)
            {
                var rewardStaticData = _rewardsData.Find(data => data.Id == rewardId);
                var receiveRewardData = receiveRewardsData.Find(data => data.Id == rewardId);

                var rewardModel = new RewardModel(rewardStaticData, receiveRewardData.Amount);
                rewardModels.Add(rewardModel);
            }

            return rewardModels;
        }
    }
}