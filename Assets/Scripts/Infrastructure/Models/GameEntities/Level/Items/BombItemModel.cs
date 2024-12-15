using Infrastructure.Data.Items;

namespace Infrastructure.Models.GameEntities.Level.Items
{
    public class BombItemModel : ItemModel
    {
        public BombItemModel(ItemData itemData) : base(itemData)
        {
            
        }

        public override bool FailOnCatch =>
            true;

        public override bool FailOnReachedArea =>
            false;
    }
}