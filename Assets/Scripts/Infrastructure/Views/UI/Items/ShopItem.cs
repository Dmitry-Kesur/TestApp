using Infrastructure.Models.GameEntities.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Items
{
    public class ShopItem : DrawableItem<ShopProductModel>
    {
        [SerializeField] private Image _purchasedMark;
        [SerializeField] private TextMeshProUGUI _priceTextField;
        [SerializeField] private Button _buyButton;

        public override void SetModel(ShopProductModel drawableModel)
        {
            base.SetModel(drawableModel);
            _buyButton.onClick.AddListener(OnBuyButtonClick);
        }

        public override void Draw()
        {
            base.Draw();
            DrawPrice();
            UpdateState();
        }

        protected override void Clear()
        {
            base.Clear();
            _buyButton.onClick.RemoveListener(OnBuyButtonClick);
        }

        private void UpdateState()
        {
            var notPurchased = !drawableModel.Purchased;

            _buyButton.enabled = notPurchased;
            _purchasedMark.gameObject.SetActive(drawableModel.Purchased);
        }

        private void DrawPrice()
        {
            _priceTextField.text = drawableModel.CostAmount.ToString();
        }

        private void OnBuyButtonClick()
        {
            drawableModel.Purchase();
        }
    }
}