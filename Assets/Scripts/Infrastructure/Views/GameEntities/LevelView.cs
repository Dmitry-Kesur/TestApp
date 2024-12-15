using DG.Tweening;
using Infrastructure.Models.GameEntities.Level;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Infrastructure.Views.GameEntities
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _catchAreaTransform;
        [SerializeField] private RectTransform _failAreaTransform;
        [SerializeField] private Image _levelBackground;
        [SerializeField] private RectTransform _containerRectTransform;
        
        private LevelModel _levelModel;

        public void SetModel(LevelModel levelModel)
        {
            _levelModel = levelModel;
            _levelModel.OnSpawnItemAction = OnSpawnItem;
            _levelBackground.sprite = _levelModel.LevelBackground;
        }

        private void OnSpawnItem(ItemView itemView)
        {
            itemView.transform.SetParent(_containerRectTransform, false);
            itemView.transform.localPosition = Vector3.zero;
            AlignItem(itemView);
            AnimateItem(itemView);
        }

        private void AnimateItem(ItemView itemView)
        {
            itemView.StartRotation();
            
            var finishPositionY = GetFinishAnimatePositionY(itemView);
            var animateDuration = _levelModel.DropItemsDuration;
            var animationTween = itemView.MoveToPosition(new Vector2(itemView.PositionX, finishPositionY),
                animateDuration);
            animationTween.OnUpdate(() => OnUpdateItem(itemView));
        }

        private float GetFinishAnimatePositionY(ItemView itemView)
        {
            var finishPositionY = _containerRectTransform.rect.yMin - itemView.Height / 2;
            return finishPositionY;
        }

        private void AlignItem(ItemView itemView)
        {
            var newPositionY = _containerRectTransform.rect.yMax - itemView.Height / 2;
            var itemPosition = new Vector2(GetRandomPositionX(itemView), newPositionY);
            itemView.transform.localPosition = itemPosition;
        }

        private float GetRandomPositionX(ItemView itemView)
        {
            var itemsContainerRect = _containerRectTransform.rect;
            var randomPositionX = Random.Range(itemsContainerRect.xMin + itemView.Width,
                itemsContainerRect.xMax - itemView.Width);
            return randomPositionX;
        }

        private void OnUpdateItem(ItemView itemView)
        {
            if (!_levelModel.Started)
                return;
            
            if (CheckReachCatchArea(itemView))
                itemView.OnReachedCatchArea();

            if (CheckReachFailArea(itemView))
                itemView.OnReachedFailArea();
        }

        private bool CheckReachCatchArea(ItemView itemView)
        {
            var itemPositionY = itemView.PositionY;
            return itemPositionY < _catchAreaTransform.localPosition.y &&
                   itemPositionY > _failAreaTransform.localPosition.y;
        }

        private bool CheckReachFailArea(ItemView itemView) =>
            itemView.PositionY < _failAreaTransform.localPosition.y;

        private void OnDisable()
        {
            _levelModel.OnSpawnItemAction = null;
        }
    }
}