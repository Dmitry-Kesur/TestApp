using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Shop;
using Infrastructure.Services.Items;
using Infrastructure.Services.Resource;

namespace Infrastructure.Services.Shop
{
    public class ShopProductRewardResolver
    {
        private readonly IItemsService _itemsService;
        private readonly ResourcesService _resourcesService;

        public ShopProductRewardResolver(IItemsService itemsService, ResourcesService resourcesService)
        {
            _itemsService = itemsService;
            _resourcesService = resourcesService;
        }

        public void Resolve(IShopProductModel iShopProduct)
        {
            var purchaseReward = iShopProduct.PurchaseReward;
            
            if (purchaseReward.Type == PurchaseRewardType.Item)
                _itemsService.UnlockItem(purchaseReward.Id);
            if (purchaseReward.Type == PurchaseRewardType.Resource)
                _resourcesService.AddResource(purchaseReward.Id, purchaseReward.Amount);
        }
    }
}