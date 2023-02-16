using DefaultNamespace;
using DefaultNamespace.UI;
using DG.Tweening;
using UI.Buttons;
using UnityEngine;

namespace UI
{
    public class ItemsSelector : BaseView
    {
        [SerializeField] private Arrow arrowLeft;
        [SerializeField] private Arrow arrowRight;
        [SerializeField] private RectTransform itemContainer;
        [SerializeField] private BaseButton selectButton;

        private int _currentIndex;
        private int _spritesCount;
        
        private ItemView _previewItem;
        private ItemModel _itemModel;

        public void Init(ItemModel itemModel)
        {
            _itemModel = itemModel;
            _currentIndex = _itemModel.activeSpriteIndex;
            _spritesCount = _itemModel.spritesCount;
            
            arrowLeft.OnArrowClickAction = OnLeftArrowClick;
            arrowRight.OnArrowClickAction = OnRightArrowClick;

            selectButton.button.onClick?.AddListener(OnSelectButtonClickHandler);

            selectButton.SetButtonText("Select");

            DrawActiveItem();
        }

        private void OnSelectButtonClickHandler()
        {
            if (_currentIndex == _itemModel.activeSpriteIndex) return;

            _itemModel.ChangeActiveSprite(_currentIndex);
            AnimateSelectedItem();
        }

        private void OnLeftArrowClick()
        {
            ChangeItemByIndex(_currentIndex - 1);
        }

        private void OnRightArrowClick()
        {
            ChangeItemByIndex(_currentIndex + 1);
        }

        private void ChangeItemByIndex(int index)
        {
            int newIndex = index;

            if (newIndex < 0)
            {
                newIndex = _spritesCount - 1;
            }

            if (newIndex >= _spritesCount)
            {
                newIndex = 0;
            }

            _currentIndex = newIndex;
            
            var sprite = _itemModel.GetSpriteByIndex(_currentIndex);
            _previewItem.SetSprite(sprite);
        }

        private void DrawActiveItem()
        {
            if (_previewItem == null)
            {
                _previewItem = _itemModel.RenderItem();    
            }
            
            _previewItem.transform.SetParent(itemContainer, false);
        }

        private void AnimateSelectedItem()
        {
            _previewItem.transform.DOScale(new Vector3(1.4f, 1.4f), 0.5f).OnComplete(() =>
            {
                _previewItem.transform.DOScale(new Vector3(1f, 1f), 0.5f);
            });
        }

        public void Clear()
        {
            selectButton.button.onClick?.RemoveListener(OnSelectButtonClickHandler);
        }
    }
}