using Infrastructure.Data.Items;

namespace Infrastructure.Models.GameEntities.Level.Items
{
    public class CandyItemModel : ItemModel
    {
        public CandyItemModel(ItemData itemData) : base(itemData)
        {
            
        }

        public override bool NeedDissolveEffect =>
            true;
    }
}