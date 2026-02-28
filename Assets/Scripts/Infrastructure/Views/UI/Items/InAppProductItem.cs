using System;
using Infrastructure.Models.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Items
{
    public class InAppProductItem : DrawableItem<InAppProductModel>
    {
        [SerializeField] private TextMeshProUGUI _payoutAmountTextField;
        [SerializeField] private TextMeshProUGUI _priceTextField;

        [SerializeField] private Button _itemButton;
        
        public override void Draw()
        {
            base.Draw();
            _priceTextField.text = drawableModel.Price;
            _payoutAmountTextField.text = drawableModel.PurchaseRewardAmount.ToString();
        }

        public override void SetModel(InAppProductModel drawableModel)
        {
            base.SetModel(drawableModel);
            _itemButton.onClick.AddListener(OnItemClicked);
        }

        protected override void Clear()
        {
            base.Clear();
            _itemButton.onClick.RemoveListener(OnItemClicked);
        }

        private void OnItemClicked() =>
            drawableModel.Purchase();
    }
}