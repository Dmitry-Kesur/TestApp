using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Services.Sound;

namespace Infrastructure.Controllers.Levels
{
    public class ItemsInteractionController
    {
        private readonly LevelProgressController _progressController;
        private readonly ISoundService _soundService;

        private List<ItemModel> _itemModels;

        public ItemsInteractionController(LevelProgressController progressController, ISoundService soundService)
        {
            _progressController = progressController;
            _soundService = soundService;
        }

        public void SetItems(List<ItemModel> itemModels)
        {
            _itemModels = itemModels;
            SubscribeListeners();
        }

        public void Clear()
        {
            UnSubscribeListeners();
            _itemModels?.Clear();
            _itemModels = null;
        }

        private void SubscribeListeners()
        {
            foreach (var itemModel in _itemModels)
            {
                itemModel.OnCatchAction = OnCatchItem;
                itemModel.OnFailAction = OnFailItem;
            }
        }

        private void UnSubscribeListeners()
        {
            if (_itemModels == null || _itemModels.Count == 0)
                return;
            
            foreach (var itemModel in _itemModels)
            {
                itemModel.OnCatchAction = null;
                itemModel.OnFailAction = null;
            }
        }

        private void OnCatchItem(ItemModel itemModel)
        {
            _progressController.UpdateProgressByCatchItem(itemModel.ScorePoints);
            _soundService.PlaySound(SoundId.Catch);
        }

        private void OnFailItem()
        {
            _progressController.UpdateProgressByFailItem();
            _soundService.PlaySound(SoundId.Fail);
        }
    }
}