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

        private bool _isCatch;
        private Tween _rotationTween;
        private Tween _moveTween;
        private ItemModel _itemModel;

        public void Init(ItemModel itemModel)
        {
            _itemModel = itemModel;
            SetSprite(itemModel.GetActiveSprite());

            itemButton.onClick.AddListener(OnItemClickHandler);
        }

        public void Clear()
        {
            StopRotation();
        }
        
        public void SetSprite(Sprite sprite)
        {
            itemIcon.sprite = sprite;
        }

        public void StartRotation()
        {
            _rotationTween = transform.DORotate(new Vector3(0, 0, 360), 8, RotateMode.FastBeyond360).SetLoops(-1);
        }

        public bool interactable
        {
            set
            {
                if (itemButton.interactable == value) return;
                itemButton.interactable = value;
            }
        }

        public bool isCatch
        {
            get => _isCatch;
            set => _isCatch = value;
        }

        public Tweener AnimateAlpha(int alpha)
        {
            return DOTween.To(value => canvasGroup.alpha = value, canvasGroup.alpha, alpha, 0.5f);
        }

        public Tween MoveToPosition(Vector2 position)
        {
            return transform.DOMove(position, 5.5f);
        }
        
        private void OnItemClickHandler()
        {
            if (isCatch) return;
         
            OnClickItemAction?.Invoke();
            _itemModel.OnCatchItem();
        }
        
        private void StopRotation()
        {
            _rotationTween?.Kill();
            _rotationTween = null;
        }
    }
}