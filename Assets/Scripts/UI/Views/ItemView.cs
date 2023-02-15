using System;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemView : BaseView
    {
        public Action OnClickItemAction;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Button itemButton;
        [SerializeField] private CanvasGroup canvasGroup;

        private bool _isCaught;
        private Tween _rotationTween;
        private Tween _moveTween;
        private ItemModel _itemModel;

        public void Init(ItemModel itemModel)
        {
            _itemModel = itemModel;
            itemIcon.sprite = itemModel.GetSprite();

            itemButton.onClick.AddListener(OnItemClickHandler);
        }

        private void OnItemClickHandler()
        {
            itemButton.onClick.RemoveListener(OnItemClickHandler);
            
            OnClickItemAction?.Invoke();
            _itemModel.OnCatchItem();
        }

        public void StartRotation()
        {
            _rotationTween = transform.DORotate(new Vector3(0, 0, 360), 8, RotateMode.FastBeyond360).SetLoops(-1);
        }

        public void StopRotation()
        {
            _rotationTween?.Kill();
            _rotationTween = null;
        }

        public bool isInteractable
        {
            get => itemButton.interactable;
            set
            {
                if (itemButton.interactable == value) return;
                itemButton.interactable = value;
            }
        }

        public bool isCaught
        {
            get => _isCaught;
            set => _isCaught = value;
        }

        public Tweener AnimateAlpha(int alpha)
        {
            return DOTween.To(value => canvasGroup.alpha = value, canvasGroup.alpha, alpha, 0.5f);
        }

        public Tween MoveToPosition(Vector2 position)
        {
            return _moveTween = transform.DOMove(position, 5.5f);
        }
    }
}