using Infrastructure.Models.GameEntities.Rewards;
using Infrastructure.Views.UI.Loaders;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Items
{
    public class RewardItem : DrawableItem<RewardModel>
    {
        [SerializeField] private TextMeshProUGUI _rewardAmountTextField;
        
        public override void Draw()
        {
            base.Draw();
            _rewardAmountTextField.text = drawableModel.Amount.ToString();
        }
    }
}