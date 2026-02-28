using Infrastructure.Models.GameEntities.DailyBonus;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Loaders;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Items
{
    public class DailyBonusDayItem : DrawableItem<DailyBonusDayModel>
    {
        [SerializeField] private TextMeshProUGUI _dayText;
        [SerializeField] private TextMeshProUGUI _rewardAmountText;
        [SerializeField] private IconLoader _rewardIconLoader;
        [SerializeField] private ButtonWithLabel _takeRewardButton;

        public override void Draw()
        {
            base.Draw();
            _dayText.text = "Day" + " " + drawableModel.Day;
            _rewardAmountText.text = "x" + drawableModel.RewardAmount;
            _rewardIconLoader.SetIconSprite(drawableModel.IconSprite);
            
            _takeRewardButton.gameObject.SetActive(drawableModel.CanTakeReward);
        }

        public override void SetModel(DailyBonusDayModel drawableModel)
        {
            base.SetModel(drawableModel);
            SubscribeListeners();
        }

        protected override void Clear()
        {
            base.Clear();
            UnsubscribeListeners();
        }

        private void SubscribeListeners()
        {
            _takeRewardButton.OnButtonClickAction += OnTakeRewardButtonClicked;
        }

        private void UnsubscribeListeners()
        {
            _takeRewardButton.OnButtonClickAction -= OnTakeRewardButtonClicked;
        }

        private void OnTakeRewardButtonClicked() => drawableModel.TakeReward();
    }
}