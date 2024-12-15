using Infrastructure.Models.GameEntities.Products.InGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Items
{
    public class ShopItem : DrawableItem<ProductModel>
    {
        [SerializeField] private Image _purchasedMark;
        [SerializeField] private TextMeshProUGUI _priceTextField;
        [SerializeField] private Button _buyButton;

        public override void SetModel(ProductModel drawableModel)
        {
            base.SetModel(drawableModel);
            _buyButton.onClick.AddListener(OnBuyButtonClick);
            drawableModel.UpdateProductAction = UpdateState;
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
            _priceTextField.text = drawableModel.Price.ToString();
        }

        private void OnBuyButtonClick()
        {
            drawableModel.Purchase();
        }
    }
}