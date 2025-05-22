using Infrastructure.Data.Items;
using Infrastructure.Models.GameEntities.Shop;

namespace Infrastructure.Models.GameEntities.Level.Items
{
    public class CandyItemModel : ItemModel, IPurchaseProductTarget
    {
        public CandyItemModel(ItemData itemData) : base(itemData)
        {
            
        }

        public override bool NeedDissolveEffect =>
            true;
        
        public void OnPurchaseComplete() =>
            OnUnlockItemAction?.Invoke(this);
    }
}