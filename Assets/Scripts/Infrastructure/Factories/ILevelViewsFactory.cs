using Infrastructure.Views.GameEntities;

namespace Infrastructure.Factories
{
    public interface ILevelViewsFactory
    {
        ItemView GetItem();
        LevelView CreateLevelView();
        void ReleaseItem(ItemView itemView);
        void Clear();
    }
}