using System;
using DG.Tweening;
using Infrastructure.Models.UI.Windows;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class BaseWindow : MonoBehaviour
    {
        [SerializeField] protected RectTransform _containerTransform;
        
        protected BaseWindowModel windowModel;

        public virtual void SetModel(BaseWindowModel model)
        {
            windowModel = model;
        }

        public virtual Type GetWindowControllerType() => null;

        public void AnimateShow() =>
            AnimateChangeScale(1);

        public void OnWindowShow()
        {
            AnimateShow();
            SubscribeListeners();
            Draw();
        }

        protected virtual void Draw()
        {
            
        }

        protected virtual void SubscribeListeners()
        {
            
        }

        protected virtual void Clear()
        {
            
        }

        private Tween AnimateChangeScale(int scaleValue) =>
            _containerTransform.DOScale(new Vector3(scaleValue, scaleValue), 0.4f);

        private void OnDestroy()
        {
            Clear();
        }
    }
}