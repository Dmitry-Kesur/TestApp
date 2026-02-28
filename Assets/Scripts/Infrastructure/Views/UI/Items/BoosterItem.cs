using Infrastructure.Models.GameEntities.Boosters;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Items
{
    public class BoosterItem : DrawableItem<BoosterModel>
    {
        [SerializeField] private TextMeshProUGUI _boostValueText;
        [SerializeField] private ButtonWithLabel _activateButton;

        public override void SetModel(BoosterModel drawableModel)
        {
            base.SetModel(drawableModel);
            _activateButton.OnButtonClickAction += drawableModel.ActivateBooster;
        }

        protected override void Clear()
        {
            base.Clear();
            if (drawableModel == null)
                return;

            _activateButton.OnButtonClickAction -= drawableModel.ActivateBooster;
        }

        public override void Draw()
        {
            base.Draw();
            _boostValueText.text = "x" + drawableModel.BoostValue;
        }
    }
}