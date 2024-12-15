using Infrastructure.Models.GameEntities.Boosters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Items
{
    public class BoosterItem : DrawableItem<BoosterModel>
    {
        [SerializeField] private TextMeshProUGUI _boostValueTextField;
        [SerializeField] private Button _buyButton;

        public override void SetModel(BoosterModel drawableModel)
        {
            base.SetModel(drawableModel);
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        public override void Draw()
        {
            base.Draw();
            _boostValueTextField.text = "x" + drawableModel.BoostValue;
        }

        protected override void Clear()
        {
            base.Clear();
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }

        private void OnBuyButtonClicked() =>
            drawableModel.OnBuyBooster();
    }
}