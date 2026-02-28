using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data;
using Infrastructure.Data.Preloader;
using Infrastructure.Data.Products;
using Infrastructure.Enums;
using Infrastructure.Factories;
using Infrastructure.Models.GameEntities.Resources;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.InAppPurchase;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress;

namespace Infrastructure.Services.Resource
{
    public class ResourcesService : IBootstrapTarget, ILoadableService
    {
        private readonly ResourcesFactory _resourcesFactory;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly LocalAddressableService _localAddressableService;
        private readonly IInAppPurchaseService _inAppPurchaseService;

        private List<ResourceData> _resourcesData;
        private List<ResourceModel> _resourceModels;

        public ResourcesService(ResourcesFactory resourcesFactory, ISaveLoadProgressService saveLoadProgressService, LocalAddressableService localAddressableService, IInAppPurchaseService inAppPurchaseService)
        {
            _resourcesFactory = resourcesFactory;
            _saveLoadProgressService = saveLoadProgressService;
            _localAddressableService = localAddressableService;
            _inAppPurchaseService = inAppPurchaseService;
            _inAppPurchaseService.OnCompletePurchaseAction += OnCompletePurchase;
        }

        public async Task Load()
        {
            _resourcesData = await _localAddressableService.LoadScriptableCollectionFromGroupAsync<ResourceData>(AddressableGroupNames
                .ResourcesGroup);   
        }

        public LoadingStage LoadingStage => LoadingStage.LoadingResources;

        public int InitializationOrder => 4;

        public void Initialize()
        {
            CreateResourceModels();
            UpdateResources();
        }

        public void AddResource(int resourceId, int amount)
        {
            var resourceModel = GetResourceById(resourceId);
            resourceModel.Amount += amount;

            if (resourceModel.Amount < 0)
                resourceModel.Amount = 0;
            
            _saveLoadProgressService.Write(progress =>
            {
                progress.ChangeResourceAmount(resourceId, resourceModel.Amount);
                
                if (resourceModel.Amount <= 0)
                    progress.RemoveResource(resourceId);
            });
        }

        public void SpendResource(int resourceId, int amount) =>
            AddResource(resourceId, -amount);

        public ResourceModel GetResourceById(int resourceId) =>
            _resourceModels.Find(resourceModel => resourceModel.Id == resourceId);

        public ResourceModel GetResourceByType(ResourceType resourceType) =>
            _resourceModels.Find(resourceModel => resourceModel.Type == resourceType);

        public List<ResourceModel> GetResourcesByType(ResourceType resourceType) =>
            _resourceModels.FindAll(resourceModel => resourceModel.Type == resourceType);

        public bool CheckEnoughResource(int resourceId, int resourceAmount)
        {
            var resourceModel = GetResourceById(resourceId);
            return resourceModel != null && resourceModel.Amount >= resourceAmount;
        }

        private void CreateResourceModels()
        {
            _resourceModels = new List<ResourceModel>();

            foreach (var resourceData in _resourcesData)
            {
                var resourceModel = _resourcesFactory.Create(resourceData);
                _resourceModels.Add(resourceModel);
            }
        }

        private void UpdateResources()
        {
            var resources = _saveLoadProgressService.Read(progress => progress.Resources);
            foreach (var resourceData in resources)
            {
                var resourceModel = GetResourceById(resourceData.resourceId);
                resourceModel.Amount = resourceData.amount;
            }
        }

        private void OnCompletePurchase(PurchaseReward purchaseReward)
        {
            if (purchaseReward.Type != PurchaseRewardType.Resource)
                return;
            
            AddResource(purchaseReward.Id, purchaseReward.Amount);
        }
    }
}