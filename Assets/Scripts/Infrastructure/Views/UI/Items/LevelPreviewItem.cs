using System;
using DG.Tweening;
using Infrastructure.Models.UI;
using Infrastructure.Models.UI.Items;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Items
{
    public class LevelPreviewItem : DrawableItem<LevelPreviewModel>
    {
        public Action<int> OnLevelSelectAction;

        [SerializeField] private ButtonWithIcon _playLevelButton;
        [SerializeField] private Image _winLevelIcon;
        [SerializeField] private Image _lockLevelIcon;
        [SerializeField] private Image _shineEffectIcon;
        [SerializeField] private RectTransform _selectHelper;
        [SerializeField] private TextMeshProUGUI _levelTextField;

        private Tween _rotationTween;

        public override void SetModel(LevelPreviewModel drawableModel)
        {
            base.SetModel(drawableModel);
            _playLevelButton.OnButtonClickAction = OnPlayButtonClick;
        }

        public override void Draw()
        {
            DrawLevelTextField();
            DrawCurrentState();
        }

        protected override void Clear()
        {
            base.Clear();
            ClearRotationTween();

            _playLevelButton.OnButtonClickAction = null;
        }

        private void DrawCurrentState()
        {
            if (drawableModel.IsComplete)
            {
                DrawCompleteState();
                return;
            }

            if (drawableModel.IsActive)
            {
                DrawActiveState();
                return;
            }

            DrawLockedState();
        }

        private void OnPlayButtonClick()
        {
            OnLevelSelectAction?.Invoke(drawableModel.Level);
        }

        private void DrawLevelTextField()
        {
            _levelTextField.text = drawableModel.Level.ToString();
        }

        private void DrawCompleteState()
        {
            _shineEffectIcon.gameObject.SetActive(false);
            _playLevelButton.gameObject.SetActive(false);
            _winLevelIcon.gameObject.SetActive(true);
        }

        private void DrawLockedState()
        {
            _shineEffectIcon.gameObject.SetActive(false);
            _playLevelButton.gameObject.SetActive(false);
            _winLevelIcon.gameObject.SetActive(false);
            _lockLevelIcon.gameObject.SetActive(true);
        }

        private void DrawActiveState()
        {
            _shineEffectIcon.gameObject.SetActive(true);
            _playLevelButton.gameObject.SetActive(true);
            _winLevelIcon.gameObject.SetActive(false);
            _lockLevelIcon.gameObject.SetActive(false);

            _rotationTween = _shineEffectIcon.transform.DORotate(new Vector3(0, 0, 360), 8, RotateMode.FastBeyond360)
                .SetLoops(-1);
        }

        private void ClearRotationTween()
        {
            _rotationTween?.Kill();
            _rotationTween = null;
        }
    }
}