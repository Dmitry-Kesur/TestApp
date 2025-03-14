using DG.Tweening;
using Infrastructure.Models.GameEntities.Level.Items;
using Shaders;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.GameEntities
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private Image _bubbleEffectImage;
        [SerializeField] private DissolveShader _dissolveShader;
        
        private bool _isCatch;

        private RectTransform _rectTransform;
        
        private Tween _movementTween;
        private Tween _rotationTween;
        private Tween _dissolveTween;

        private ItemModel _itemModel;

        private void Awake() =>
            _rectTransform ??= GetComponent<RectTransform>();

        public bool Paused { get; set; }
        
        private void OnEnable()
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnClickHandler);
            _button.enabled = true;
        }

        public void SetModel(ItemModel itemModel) =>
            _itemModel = itemModel;

        public virtual void Draw()
        {
            _icon.sprite = _itemModel.GetSprite();
            ChangeBubbleImageVisibility(true);
            EnableInteraction = false;
            
            _dissolveShader.UpdateDissolveShader();
            _dissolveShader.DissolveAmount = 0;
        }

        public void PauseAnimations()
        {
            Paused = true;
            _rotationTween.Pause();
            _movementTween.Pause();
            _dissolveTween.Pause();
        }

        public void ResumeAnimations()
        {
            Paused = false;
            _rotationTween.Play();
            _movementTween.Play();
            _dissolveTween.Play();
        }

        public void StartRotation()
        {
            ClearRotation();
            _rotationTween = transform.DORotate(new Vector3(0, 0, 360), 8, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }

        public void OnReachedCatchArea()
        {
            if (_isCatch || EnableInteraction)
                return;
            
            ChangeBubbleImageVisibility(false);
            EnableInteraction = true;
        }

        public virtual void OnReachedFailArea()
        {
            if (_isCatch || !EnableInteraction)
                return;
            
            EnableInteraction = false;
            RemoveItem();

            if (_itemModel.FailOnReachedArea)
                _itemModel.OnFail();
        }

        public Tween MoveToPosition(Vector2 position, float duration)
        {
            _movementTween = transform.DOLocalMove(position, duration);
            return _movementTween;
        }

        public float PositionY =>
            transform.localPosition.y;

        public float PositionX =>
            transform.localPosition.x;

        public float Width =>
            _rectTransform.rect.width;

        public float Height =>
            _rectTransform.rect.height;

        private void OnCatch()
        {
            if (_itemModel.FailOnCatch)
            {
                RemoveItem();
                _itemModel.OnFail();
                return;
            }
            
            _itemModel.OnCatch();
            
            PlayDissolveEffect();
        }

        protected virtual void Clear()
        {
            ClearRotation();
            ClearMovement();
            ClearDissolve();

            EnableInteraction = false;
            _isCatch = false;
            
            _button.onClick.RemoveAllListeners();
        }

        private void PlayDissolveEffect()
        {
            if (!_itemModel.NeedDissolveEffect)
                return;
            
            _dissolveTween = DOTween.To(() => _dissolveShader.DissolveAmount, amount => _dissolveShader.DissolveAmount = amount, 1, 1f);
            _dissolveTween.OnComplete(RemoveItem);
        }

        private void RemoveItem()
        {
            Clear();
            _itemModel.OnRemoveItem(this);
        }

        private bool EnableInteraction { get; set; }

        private void ChangeBubbleImageVisibility(bool visibility) =>
            _bubbleEffectImage.gameObject.SetActive(visibility);

        private void OnClickHandler()
        {
            if (_isCatch)
                return;
            
            EnableInteraction = false;
            _isCatch = true;
            _button.enabled = false;
            OnCatch();
        }

        private void ClearRotation()
        {
            if (_rotationTween == null)
                return;

            _rotationTween?.Kill();
            _rotationTween = null;
            transform.rotation = Quaternion.identity;
        }

        private void ClearMovement()
        {
            _movementTween?.Kill();
            _movementTween = null;
        }
        
        private void ClearDissolve()
        {
            _dissolveTween.Kill();
            _dissolveTween = null;
        }

        private void OnDestroy() =>
            Clear();
    }
}