using Infrastructure.Views.GameEntities;

namespace Infrastructure.Factories.Level
{
    public interface IItemViewsFactory
    {
        public ItemView GetItem();
        public void ReleaseItem(ItemView itemView);
        public void Clear();
    }
}