using System;
using Infrastructure.Models.UI;
using Infrastructure.Views.UI.Items;

namespace Infrastructure.Views.UI.Loaders
{
    public class LevelsLoader : LoaderWithCache<LevelPreviewModel>
    {
        public Action<int> OnLevelSelectAction;

        protected override void SubscribeItemListeners(DrawableItem<LevelPreviewModel> drawableItem)
        {
            base.SubscribeItemListeners(drawableItem);

            if (drawableItem is LevelPreviewItem levelSelectItem)
            {
                levelSelectItem.OnLevelSelectAction = OnLevelSelect;
            }
        }

        private void OnLevelSelect(int level)
        {
            OnLevelSelectAction?.Invoke(level);
        }
    }
}