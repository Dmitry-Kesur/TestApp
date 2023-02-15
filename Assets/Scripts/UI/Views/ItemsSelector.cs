using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.UI;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class ItemsSelector : BaseView
    {
        [SerializeField] private Arrow arrowLeft;
        [SerializeField] private Arrow arrowRight;
        [SerializeField] private RectTransform itemContainer;
        
        private ItemView _currentItem;
        private List<ItemModel> _items;
        private int _currentIndex;

        public void Init(List<ItemModel> items)
        {
            _items = items;
            arrowLeft.OnArrowClickAction = OnLeftArrowClick;
            arrowRight.OnArrowClickAction = OnRightArrowClick;

            DrawActiveItem();
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
                newIndex = _items.Count - 1;
            }

            if (newIndex >= _items.Count)
            {
                newIndex = 0;
            }

            _currentIndex = newIndex;

            RemoveCurrentItem();

            var itemModel = _items.Find(model => model.id == _currentIndex);
            _currentItem = itemModel.RenderItem();
            _currentItem.transform.SetParent(itemContainer, false);
        }

        private void RemoveCurrentItem()
        {
            Destroy(_currentItem.gameObject);
            _currentItem = null;
        }

        private void DrawActiveItem()
        {
            var itemModel = _items.Find(model => model.isSelected);
            _currentIndex = itemModel.id;

            _currentItem = itemModel.RenderItem();
            _currentItem.transform.SetParent(itemContainer, false);
        }

        public int selectedItemId => _currentIndex;

        public void AnimateSelectedItem()
        {
            _currentItem.transform.DOScale(new Vector3(1.4f, 1.4f), 0.5f).OnComplete(() =>
            {
                _currentItem.transform.DOScale(new Vector3(1f, 1f), 0.5f);
            });
        }
    }
}