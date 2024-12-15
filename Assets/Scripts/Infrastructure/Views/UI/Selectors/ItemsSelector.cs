using System;
using System.Collections.Generic;
using DG.Tweening;
using Infrastructure.Models.GameEntities.Level.Items;
using Infrastructure.Views.UI.Arrows;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Items;
using UnityEngine;

namespace Infrastructure.Views.UI.Selectors
{
    public class ItemsSelector : MonoBehaviour
    {
        public Action<int> OnItemSelectAction;

        [SerializeField] private Arrow arrowLeft;
        [SerializeField] private Arrow arrowRight;
        [SerializeField] private PreviewSelectItem previewSelectItem;
        [SerializeField] private ButtonWithLabel selectButton;
        [SerializeField] private ButtonWithIcon resetSelectionButton;

        private int _currentItemIndex;
        private int _selectedItemId;

        private List<ItemModel> _itemModels;

        public void Init(List<ItemModel> itemModels, int selectedItemId)
        {
            _itemModels = itemModels;
            Clear();

            UpdateSelectedItem(selectedItemId);

            selectButton.SetButtonText("Select");
            
            UpdatePreviewSelectItem();

            SubscribeListeners();
        }

        private void UpdateSelectedItem(int selectedItemId)
        {
            var hasSelectedItem = selectedItemId > 0;
            if (hasSelectedItem)
            {
                var itemModel = _itemModels.Find(itemModel => itemModel.Id == selectedItemId);
                _selectedItemId = itemModel.Id;
                _currentItemIndex = GetIndexByItemModel(itemModel);
            }
            else
            {
                _currentItemIndex = 0;
            }

            ChangeResetSelectionButtonVisibility(hasSelectedItem);
        }

        private int GetIndexByItemModel(ItemModel itemModel) =>
            _itemModels.IndexOf(itemModel);

        private void Clear()
        {
            _currentItemIndex = 0;
        }

        private void SubscribeListeners()
        {
            arrowLeft.OnArrowClickAction = OnLeftArrowClick;
            arrowRight.OnArrowClickAction = OnRightArrowClick;
            resetSelectionButton.OnButtonClickAction = OnResetItemSelectionButtonClickHandler;
            selectButton.OnButtonClickAction = OnSelectButtonClickHandler;
        }

        private void OnSelectButtonClickHandler()
        {
            var itemModelByIndex = _itemModels[_currentItemIndex];
            if (_selectedItemId == itemModelByIndex.Id) return;
            _selectedItemId = itemModelByIndex.Id;
            
            ChangePreviewItemSprite(itemModelByIndex.GetSprite());

            ChangeResetSelectionButtonVisibility(true);
            AnimateSelectedItem();

            OnItemSelectAction?.Invoke(itemModelByIndex.Id);
        }

        private void OnResetItemSelectionButtonClickHandler()
        {
            var itemModel = GetItemByIndex(0);
            var newItemIndex = GetIndexByItemModel(itemModel);
          
            _selectedItemId = 0;
            _currentItemIndex = newItemIndex;

            UpdatePreviewSelectItem();
            ChangeResetSelectionButtonVisibility(false);

            OnItemSelectAction?.Invoke(0);
        }

        private void UpdateResetSelectionButton()
        {
            var itemModelByIndex = _itemModels[_currentItemIndex];
            var needToShowResetSelectionButton = _selectedItemId == itemModelByIndex.Id;
            ChangeResetSelectionButtonVisibility(needToShowResetSelectionButton);
        }

        private void ChangeResetSelectionButtonVisibility(bool visibility)
        {
            resetSelectionButton.gameObject.SetActive(visibility);
        }

        private void OnLeftArrowClick()
        {
            ChangeItemById(_currentItemIndex - 1);
        }

        private void OnRightArrowClick()
        {
            ChangeItemById(_currentItemIndex + 1);
        }

        private void ChangeItemById(int id)
        {
            int newId = id;

            if (newId < 0)
            {
                newId = _itemModels.Count - 1;
            }

            if (newId >= _itemModels.Count)
            {
                newId = 0;
            }

            _currentItemIndex = newId;

            var itemModel = GetItemByIndex(_currentItemIndex);
            var sprite = itemModel.GetSprite();
            ChangePreviewItemSprite(sprite);

            UpdateResetSelectionButton();
        }

        private ItemModel GetItemByIndex(int index) =>
            _itemModels[index];

        private void UpdatePreviewSelectItem()
        {
            var itemModel = GetItemByIndex(_currentItemIndex);
            ChangePreviewItemSprite(itemModel?.GetSprite());
        }

        private void ChangePreviewItemSprite(Sprite sprite)
        {
            previewSelectItem.SetSprite(sprite);
        }

        private void AnimateSelectedItem()
        {
            previewSelectItem.transform.DOScale(new Vector3(1.4f, 1.4f), 0.5f).OnComplete(() =>
            {
                previewSelectItem.transform.DOScale(new Vector3(1f, 1f), 0.5f);
            });
        }
    }
}